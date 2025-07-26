using System.Data;
using FluentMigrator;
using Platform.Domain;

namespace Catalog.Domain.Migrations;

[Migration(0)]
public class M0000_CreateDatabase : Migration
{
    public override void Up()
    {
        CreateIngredients();
        CreatePizzas();
    }

    public override void Down()
    {
        throw new NotSupportedException();
    }

    private void CreateIngredients()
    {
        this.Create.Table("Ingredients")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Version").AsInt32().NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("ImageId").AsInt32().NotNullable()
            .WithColumn("ImageUrl").AsString().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("PizzeriaAccountId").AsInt32().NotNullable()
            .WithColumn("Price").AsDecimal().NotNullable();
    }

    private void CreatePizzas()
    {
        this.Create.Table("Pizzas")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Version").AsInt32().NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Description").AsString(300).NotNullable()
            .WithColumn("ImageId").AsInt32().NotNullable()
            .WithColumn("ImageUrl").AsString().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("PizzeriaAccountId").AsInt32().NotNullable();

        this.Create.Table("PizzaPrices")
            .WithColumn("PizzaId").AsInt32().NotNullable().ForeignKey("Pizzas", "Id").OnDelete(Rule.Cascade).PrimaryKey()
            .WithColumn("Price").AsDecimal().NotNullable()
            .WithColumn("Size").AsInt32().NotNullable().PrimaryKey();

        this.Create.Table("PizzaIngredients")
            .WithColumn("PizzaId").AsInt32().NotNullable().ForeignKey("Pizzas", "Id").OnDelete(Rule.Cascade).PrimaryKey()
            .WithColumn("IngredientId").AsInt32().NotNullable().ForeignKey("Ingredients", "Id").OnDelete(Rule.Cascade).PrimaryKey();
    }
}