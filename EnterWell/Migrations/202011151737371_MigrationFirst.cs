namespace EnterWell.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFirst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ItemId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ShoppingCartViewModel_id = c.Int(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingCartViewModels", t => t.ShoppingCartViewModel_id)
                .Index(t => t.ItemId)
                .Index(t => t.ShoppingCartViewModel_id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        ProducerId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 160),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemArtUrl = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Producers", t => t.ProducerId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.ProducerId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        ProducerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProducerId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        OrderDueDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalWithPDV = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.ShoppingCartViewModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CartTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "ShoppingCartViewModel_id", "dbo.ShoppingCartViewModels");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Carts", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "ProducerId" });
            DropIndex("dbo.Items", new[] { "CategoryId" });
            DropIndex("dbo.Carts", new[] { "ShoppingCartViewModel_id" });
            DropIndex("dbo.Carts", new[] { "ItemId" });
            DropTable("dbo.ShoppingCartViewModels");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Producers");
            DropTable("dbo.Categories");
            DropTable("dbo.Items");
            DropTable("dbo.Carts");
        }
    }
}
