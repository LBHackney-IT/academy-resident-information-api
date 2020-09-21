using Microsoft.EntityFrameworkCore.Migrations;

namespace AcademyResidentInformationApi.Migrations
{
    public partial class AddInitialIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
            migrationBuilder.Sql("CREATE INDEX hbmember_forename_idx ON dbo.hbmember USING btree (forename);");
            migrationBuilder.Sql("CREATE INDEX hbmember_surname_idx ON dbo.hbmember USING btree (surname);");
            migrationBuilder.Sql("CREATE INDEX hbmember_forename_ilike_idx ON dbo.hbmember USING gin (forename gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX hbmember_surname_ilike_idx ON dbo.hbmember USING gin (surname gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX hbhousehold_post_code_idx ON dbo.hbhousehold USING gin (post_code gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX hbhousehold_addr1_idx ON dbo.hbhousehold USING gin (addr1 gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX hbhousehold_to_date_idx ON dbo.hbhousehold USING btree (to_date);");
            migrationBuilder.Sql("CREATE INDEX hbhousehold_postcode_whitespace_replaced_idx ON dbo.hbhousehold USING gin (replace((post_code)::text, ' '::text, ''::text) gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX hbhousehold_address_whitespace_replaced_idx ON dbo.hbhousehold USING gin (replace((addr1)::text, ' '::text, ''::text) gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX uc_hbmember ON dbo.hbmember USING btree (claim_id, house_id);");
            migrationBuilder.Sql("CREATE INDEX ctoccupation_account_ref ON dbo.ctoccupation USING btree (account_ref);");
            migrationBuilder.Sql("CREATE INDEX ctoccupation_vacation_date ON dbo.ctoccupation USING btree (vacation_date);");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbmember_forename_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbmember_surname_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbmember_forename_ilike_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbmember_surname_ilike_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbhousehold_post_code_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbhousehold_addr1_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbhousehold_to_date_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbhousehold_postcode_whitespace_replaced_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.hbhousehold_address_whitespace_replaced_idx;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.uc_hbmember;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.ctoccupation_account_ref;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS dbo.ctoccupation_vacation_date;");

        }
    }
}
