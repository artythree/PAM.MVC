namespace PAM.DataN.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NeuronLayerOne",
                c => new
                    {
                        NeuronLayerOneId = c.Int(nullable: false, identity: true),
                        PerceptronId = c.Int(nullable: false),
                        PercentChange = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.NeuronLayerOneId)
                .ForeignKey("dbo.Perceptron", t => t.PerceptronId, cascadeDelete: true)
                .Index(t => t.PerceptronId);
            
            CreateTable(
                "dbo.Perceptron",
                c => new
                    {
                        PerceptronId = c.Int(nullable: false, identity: true),
                        Stock = c.String(),
                        Error = c.Double(nullable: false),
                        Output = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PerceptronId);
            
            CreateTable(
                "dbo.Neuron",
                c => new
                    {
                        NeuronId = c.Int(nullable: false, identity: true),
                        PerceptronId = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        PercentChange = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.NeuronId)
                .ForeignKey("dbo.Perceptron", t => t.PerceptronId, cascadeDelete: true)
                .Index(t => t.PerceptronId);
            
            CreateTable(
                "dbo.WeightJoiningTableLayerOne",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NeuronId = c.Int(),
                        NeuronLayerOneId = c.Int(),
                        Weight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Neuron", t => t.NeuronId, cascadeDelete: true)
                .ForeignKey("dbo.NeuronLayerOne", t => t.NeuronLayerOneId, cascadeDelete: true)
                .Index(t => t.NeuronId)
                .Index(t => t.NeuronLayerOneId);
            
            CreateTable(
                "dbo.NeuronLayerTwo",
                c => new
                    {
                        NeuronLayerTwoId = c.Int(nullable: false, identity: true),
                        PerceptronId = c.Int(nullable: false),
                        PercentChange = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.NeuronLayerTwoId)
                .ForeignKey("dbo.Perceptron", t => t.PerceptronId, cascadeDelete: true)
                .Index(t => t.PerceptronId);
            
            CreateTable(
                "dbo.WeightJoiningTableLayerTwo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NeuronLayerTwoId = c.Int(),
                        NeuronLayerOneId = c.Int(),
                        Weight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NeuronLayerOne", t => t.NeuronLayerOneId, cascadeDelete:true)
                .ForeignKey("dbo.Neuron", t => t.NeuronLayerTwoId, cascadeDelete: true)
                .ForeignKey("dbo.NeuronLayerTwo", t => t.NeuronLayerTwoId, cascadeDelete: true)
                .Index(t => t.NeuronLayerTwoId)
                .Index(t => t.NeuronLayerOneId);
            
            CreateTable(
                "dbo.MarketData",
                c => new
                    {
                        Ticker = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        PercentChange = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticker, t.Date });
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Search",
                c => new
                    {
                        SearchId = c.Int(nullable: false, identity: true),
                        Index = c.String(),
                        Stock = c.String(),
                        DateOfSearch = c.DateTimeOffset(nullable: false, precision: 7),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SearchId)
                .ForeignKey("dbo.ApplicationUser", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Search", "UserId", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.NeuronLayerOne", "PerceptronId", "dbo.Perceptron");
            DropForeignKey("dbo.WeightJoiningTableLayerTwo", "NeuronLayerTwoId", "dbo.NeuronLayerTwo");
            DropForeignKey("dbo.WeightJoiningTableLayerTwo", "NeuronLayerTwoId", "dbo.Neuron");
            DropForeignKey("dbo.WeightJoiningTableLayerTwo", "NeuronLayerOneId", "dbo.NeuronLayerOne");
            DropForeignKey("dbo.NeuronLayerTwo", "PerceptronId", "dbo.Perceptron");
            DropForeignKey("dbo.WeightJoiningTableLayerOne", "NeuronLayerOneId", "dbo.NeuronLayerOne");
            DropForeignKey("dbo.WeightJoiningTableLayerOne", "NeuronId", "dbo.Neuron");
            DropForeignKey("dbo.Neuron", "PerceptronId", "dbo.Perceptron");
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Search", new[] { "UserId" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.WeightJoiningTableLayerTwo", new[] { "NeuronLayerOneId" });
            DropIndex("dbo.WeightJoiningTableLayerTwo", new[] { "NeuronLayerTwoId" });
            DropIndex("dbo.NeuronLayerTwo", new[] { "PerceptronId" });
            DropIndex("dbo.WeightJoiningTableLayerOne", new[] { "NeuronLayerOneId" });
            DropIndex("dbo.WeightJoiningTableLayerOne", new[] { "NeuronId" });
            DropIndex("dbo.Neuron", new[] { "PerceptronId" });
            DropIndex("dbo.NeuronLayerOne", new[] { "PerceptronId" });
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.Search");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.MarketData");
            DropTable("dbo.WeightJoiningTableLayerTwo");
            DropTable("dbo.NeuronLayerTwo");
            DropTable("dbo.WeightJoiningTableLayerOne");
            DropTable("dbo.Neuron");
            DropTable("dbo.Perceptron");
            DropTable("dbo.NeuronLayerOne");
        }
    }
}
