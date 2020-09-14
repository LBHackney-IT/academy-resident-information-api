using FluentMigrator;

namespace AcademyResidentInformationApi.V1.Infrastructure.Migrations
{
    [Migration(202009141648)]
    public class AddInitialIndexes : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm");
            Execute.Sql("CREATE UNIQUE INDEX hbclaim_pkey ON dbo.hbclaim USING btree (claim_id);");
            Execute.Sql("CREATE UNIQUE INDEX hbhousehold_pkey ON dbo.hbhousehold USING btree (claim_id, house_id);");
            Execute.Sql("CREATE UNIQUE INDEX hbmember_pkey ON dbo.hbmember USING btree (claim_id, house_id, member_id);");
            Execute.Sql("CREATE INDEX hbmember_forename_idx ON dbo.hbmember USING btree (forename);");
            Execute.Sql("CREATE INDEX hbmember_surname_idx ON dbo.hbmember USING btree (surname);");
            Execute.Sql("CREATE INDEX hbmember_forename_ilike_idx ON dbo.hbmember USING gin (forename gin_trgm_ops);");
            Execute.Sql("CREATE INDEX hbmember_surname_ilike_idx ON dbo.hbmember USING gin (surname gin_trgm_ops);");
            Execute.Sql("CREATE INDEX hbhousehold_post_code_idx ON dbo.hbhousehold USING gin (post_code gin_trgm_ops);");
            Execute.Sql("CREATE INDEX hbhousehold_addr1_idx ON dbo.hbhousehold USING gin (addr1 gin_trgm_ops);");
            Execute.Sql("CREATE INDEX hbhousehold_to_date_idx ON dbo.hbhousehold USING btree (to_date);");
            Execute.Sql("CREATE INDEX hbhousehold_postcode_whitespace_replaced_idx ON dbo.hbhousehold USING gin (replace((post_code)::text, ' '::text, ''::text) gin_trgm_ops);");
            Execute.Sql("CREATE INDEX hbhousehold_address_whitespace_replaced_idx ON dbo.hbhousehold USING gin (replace((addr1)::text, ' '::text, ''::text) gin_trgm_ops);");
        }

        public override void Down()
        {
            Execute.Sql("DROP INDEX IF EXISTS ");
            Execute.Sql("DROP INDEX IF EXISTS hbclaim_pkey;");
            Execute.Sql("DROP INDEX IF EXISTS hbhousehold_pkey;");
            Execute.Sql("DROP INDEX IF EXISTS hbmember_pkey;");
            Execute.Sql("DROP INDEX IF EXISTS hbmember_forename_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbmember_surname_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbmember_forename_ilike_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbmember_surname_ilike_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbhousehold_post_code_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbhousehold_addr1_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbhousehold_to_date_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbhousehold_postcode_whitespace_replaced_idx;");
            Execute.Sql("DROP INDEX IF EXISTS hbhousehold_address_whitespace_replaced_idx;");
        }
    }
}
