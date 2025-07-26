using System.Text;
using Auth.Domain.User;
using FluentMigrator;
using Utilities;
using Platform.Domain;

namespace Auth.Domain.Migrations;

[Migration(0)]
public class M0000_CreateDatabase : Migration
{
    public override void Up()
    {
        this.Create.Table("Users")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Version").AsInt32().NotNullable()
            .WithColumn("Login").AsString().NotNullable()
            .WithColumn("PasswordHash").AsBinary(32).NotNullable()
            .WithColumn("Role").AsByte().NotNullable();
        
        byte[] password = Encoding.UTF8.GetBytes("123123");
        
        this.Insert.IntoTable("Users").Row(
            new
            {
                Login = "admin", 
                Version = 0,
                PasswordHash = password, 
                Role = (byte)UserRole.Admin
            });
    }

    public override void Down()
    {
        throw new NotSupportedException();
    }
}