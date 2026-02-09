namespace Project_ecomm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 500),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPrice = c.Decimal(precision: 18, scale: 2),
                        ImageUrl = c.String(maxLength: 250),
                        StockQuantity = c.Int(nullable: false),
                        Category = c.String(maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
