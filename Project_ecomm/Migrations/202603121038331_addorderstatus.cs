namespace Project_ecomm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addorderstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Status");
        }
    }
}
