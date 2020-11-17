namespace EnterWell.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migggi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingCartViewModels", "CartTotalWithPDV", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoppingCartViewModels", "CartTotalWithPDV");
        }
    }
}
