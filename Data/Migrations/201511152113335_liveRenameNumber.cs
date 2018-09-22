namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class liveRenameNumber : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fixtures", newName: "ChampionFixture2");
            RenameTable(name: "dbo.UserScores", newName: "UserChampionScore4");
            DropForeignKey("dbo.Predictions", "FixtureId", "dbo.Fixtures");
            DropIndex("dbo.Predictions", new[] { "FixtureId" });
            CreateTable(
                "dbo.ChampionPrediction2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePosted = c.DateTime(nullable: false),
                        HomePrediction = c.Int(nullable: false),
                        AwayPrediction = c.Int(nullable: false),
                        ChampionFixtureID = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChampionFixture2", t => t.ChampionFixtureID, cascadeDelete: true)
                .Index(t => t.ChampionFixtureID);
            
            CreateTable(
                "dbo.PremierFixture3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FixtureDate = c.DateTime(nullable: false),
                        HomeTeam = c.String(),
                        AwayTeam = c.String(),
                        HomeScore = c.Int(nullable: false),
                        AwayScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PremierPrediction3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePosted = c.DateTime(nullable: false),
                        HomePrediction = c.Int(nullable: false),
                        AwayPrediction = c.Int(nullable: false),
                        PremierFixtureID = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PremierFixture3", t => t.PremierFixtureID, cascadeDelete: true)
                .Index(t => t.PremierFixtureID);
            
            CreateTable(
                "dbo.UserPremierScore4",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Score = c.Int(nullable: false),
                        ExactPredictions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.Predictions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Predictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePosted = c.DateTime(nullable: false),
                        HomePrediction = c.Int(nullable: false),
                        AwayPrediction = c.Int(nullable: false),
                        FixtureId = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.PremierPrediction3", "PremierFixtureID", "dbo.PremierFixture3");
            DropForeignKey("dbo.ChampionPrediction2", "ChampionFixtureID", "dbo.ChampionFixture2");
            DropIndex("dbo.PremierPrediction3", new[] { "PremierFixtureID" });
            DropIndex("dbo.ChampionPrediction2", new[] { "ChampionFixtureID" });
            DropTable("dbo.UserPremierScore4");
            DropTable("dbo.PremierPrediction3");
            DropTable("dbo.PremierFixture3");
            DropTable("dbo.ChampionPrediction2");
            CreateIndex("dbo.Predictions", "FixtureId");
            AddForeignKey("dbo.Predictions", "FixtureId", "dbo.Fixtures", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UserChampionScore4", newName: "UserScores");
            RenameTable(name: "dbo.ChampionFixture2", newName: "Fixtures");
        }
    }
}
