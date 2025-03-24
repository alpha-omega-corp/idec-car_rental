using FluentMigrator;

namespace CarRental.Database.Migrations;

[Migration(1)]
public class CreateUserTable : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString().Unique()
            .WithColumn("email").AsString().Unique()
            .WithColumn("password").AsString();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}