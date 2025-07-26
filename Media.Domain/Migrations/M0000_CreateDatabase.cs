using System.Data;
using FluentMigrator;

namespace Media.Domain.Migrations;

[Migration(0)]
public class M0000_CreateDatabase : Migration
{
    public override void Up()
    {
        this.Create.Table("Images")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Version").AsInt32().NotNullable()
            .WithColumn("Url").AsString().NotNullable()
            .WithColumn("PublicId").AsString().NotNullable()
            .WithColumn("Folder").AsByte().NotNullable();
    }

    public override void Down()
    {
        throw new NotSupportedException();
    }
}