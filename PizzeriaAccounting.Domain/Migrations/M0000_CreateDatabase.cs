using System.Data;
using FluentMigrator;

namespace Auth.Domain.Migrations;

[Migration(0)]
public class M0000_CreateDatabase : Migration
{
    public override void Up()
    {
        this.Create.Table("Accounts")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Version").AsInt32().NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Address").AsString(100).NotNullable()
            .WithColumn("Phone").AsString(50).NotNullable()
            .WithColumn("Email").AsString(50).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable();

        this.Create.Table("AccountUsers")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Version").AsInt32().NotNullable()
            .WithColumn("Email").AsString(50).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("AccountId").AsInt32().NotNullable().ForeignKey("Accounts", "Id").OnDelete(Rule.Cascade)
            .WithColumn("UserId").AsInt32().NotNullable();
        
        this.Insert.IntoTable("Accounts").Row(
            new
            {
                Name = "Test account", 
                Address = "Test address",
                Phone = "8-9999992255", 
                Email = "test@testpizza.io",
                IsActive = true
            });
    }

    public override void Down()
    {
        throw new NotSupportedException();
    }
}