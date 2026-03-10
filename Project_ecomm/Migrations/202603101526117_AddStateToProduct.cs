namespace Project_ecomm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "State", c => c.String(nullable: false, maxLength: 150));
            DropColumn("dbo.Products", "DiscountPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "DiscountPrice", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Products", "State");
        }
    }
}
