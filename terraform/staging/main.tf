# INSTRUCTIONS:
# 1) ENSURE YOU POPULATE THE LOCALS
# 2) ENSURE YOU REPLACE ALL INPUT PARAMETERS, THAT CURRENTLY STATE 'ENTER VALUE', WITH VALID VALUES
# 3) YOUR CODE WOULD NOT COMPILE IF STEP NUMBER 2 IS NOT PERFORMED!
# 4) ENSURE YOU CREATE A BUCKET FOR YOUR STATE FILE AND YOU ADD THE NAME BELOW - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL - FOR APIS, THE BUCKETS ALREADY EXIST
# 5) THE VALUES OF THE COMMON COMPONENTS THAT YOU WILL NEED ARE PROVIDED IN THE COMMENTS
# 6) IF ADDITIONAL RESOURCES ARE REQUIRED BY YOUR API, ADD THEM TO THIS FILE
# 7) ENSURE THIS FILE IS PLACED WITHIN A 'terraform' FOLDER LOCATED AT THE ROOT PROJECT DIRECTORY
provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
   //application_name = your application name # The name to use for your application
   parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}
data "aws_iam_role" "ec2_container_service_role" {
  name = "ecsServiceRole"
}
data "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRole"
}
terraform {
  backend "s3" {
    bucket  = "terraform-state-staging-apis"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/academy-information-api/state"
  }
}
/*    POSTGRES SET UP    */
data "aws_vpc" "staging_vpc" {
  tags = {
    Name = "vpc-staging-apis-staging"
  }
}
data "aws_subnet_ids" "staging" {
  vpc_id = data.aws_vpc.staging_vpc.id
 filter {
    name   = "tag:Type"
    values = ["private"] # insert values here
  }
}
 data "aws_ssm_parameter" "academy_postgres_db_password" {
   name = "/academy-api/staging/postgres-password"
 }
  data "aws_ssm_parameter" "academy_postgres_username" {
   name = "/academy-api/staging/postgres-username"
 }
 data "aws_ssm_parameter" "academy_postgres_hostname" {
   name = "/academy-api/staging/postgres-hostname"
 }
module "postgres_db_staging" {
  source = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/postgres"
  environment_name = "staging"
  vpc_id = data.aws_vpc.staging_vpc.id
  db_identifier = "academy-mirror-db"
  db_name = "academy_mirror"
  db_port  = 5102
  subnet_ids = data.aws_subnet_ids.staging.ids
  db_engine = "postgres"
  db_engine_version = "11.1"
  db_instance_class = "db.t2.micro"
  db_allocated_storage = 20
  maintenance_window = "sun:10:00-sun:10:30"
  db_username = data.aws_ssm_parameter.academy_postgres_username.value
  db_password = data.aws_ssm_parameter.academy_postgres_db_password.value
  storage_encrypted = false
  multi_az = false //only true if production deployment
  publicly_accessible = false
  project_name = "platform apis"
}
data "aws_ssm_parameter" "academy_username" {
   name = "/academy/reporting-server/username"
}
data "aws_ssm_parameter" "academy_password" {
   name = "/academy/reporting-server/password"
}
data "aws_ssm_parameter" "academy_hostname" {
   name = "/academy/reporting-server/hostname"
}
module "dms_setup_staging" {
  source = "github.com/LBHackney-IT/aws-dms-terraform.git//dms_setup_existing_instance"
  environment_name = "staging" //used for resource tags
  project_name = "resident-information-apis" //used for resource tags
  //target db for dms endpoint
  target_db_name = "academy_mirror" //the name of the target database
  target_endpoint_identifier = "target-academy-endpoint" //unique identifier (name) you give for the endpoint to be created
  target_db_engine_name = "postgres"
  target_db_port = 5102
  target_db_username = data.aws_ssm_parameter.academy_postgres_username.value//ensure you save your Postgres db credentials to the Parameter store and reference it here
  target_db_password = data.aws_ssm_parameter.academy_postgres_db_password.value//ensure you save your Postgres db credentials to the Parameter store and reference it here
  target_db_server = data.aws_ssm_parameter.academy_postgres_hostname.value //Postgres instance endpoint
  target_endpoint_ssl_mode = "none"
  //source db for dms endpoint
  source_db_name = "HBCTLIVEDB" //the name of the source (on-prem) database
  source_endpoint_identifier = "source-academy-endpoint" //unique identifier (name) you give for the endpoint to be created
  source_db_engine_name = "sqlserver"
  source_db_port = 1433
  source_db_username = data.aws_ssm_parameter.academy_username.value //ensure you save your on-prem credentials to the Parameter store and reference it here
  source_db_password = data.aws_ssm_parameter.academy_password.value //ensure you save your on-prem credentials to the Parameter store and reference it here
  source_db_server = data.aws_ssm_parameter.academy_hostname.value //your on-prem db IP
  source_endpoint_ssl_mode = "none"
  //dms task set up
  migration_type = "full-load" //full-load | cdc | full-load-and-cdc -> full-load-and-cdc is the preferred option, it does the one-off migration and uses the CDC option for continous migration
  replication_task_indentifier = "academy-api-dms-task" //unique identifier (name) you give for the instance to be created
  task_settings = file("${path.module}/task_settings.json") //path to your json file with task settings
  task_table_mappings = file("${path.module}/selection_rules.json") //path to your json file with selection rules
  replication_instance_arn = "arn:aws:dms:${data.aws_region.current}:${data.aws_caller_identity.current.account_id}:rep:parameter"
}
