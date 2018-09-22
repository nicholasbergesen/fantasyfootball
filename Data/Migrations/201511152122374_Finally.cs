namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Finally : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChampionPredictions",
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
                .ForeignKey("dbo.ChampionFixtures", t => t.ChampionFixtureID, cascadeDelete: true)
                .Index(t => t.ChampionFixtureID);
            
            CreateTable(
                "dbo.PremierFixtures",
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
                "dbo.PremierPredictions",
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
                .ForeignKey("dbo.PremierFixtures", t => t.PremierFixtureID, cascadeDelete: true)
                .Index(t => t.PremierFixtureID);
            
            CreateTable(
                "dbo.UserPremierScores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Score = c.Int(nullable: false),
                        ExactPredictions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
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
            
            DropForeignKey("dbo.PremierPredictions", "PremierFixtureID", "dbo.PremierFixtures");
            DropForeignKey("dbo.ChampionPredictions", "ChampionFixtureID", "dbo.ChampionFixtures");
            DropIndex("dbo.PremierPredictions", new[] { "PremierFixtureID" });
            DropIndex("dbo.ChampionPredictions", new[] { "ChampionFixtureID" });
            DropTable("dbo.UserPremierScores");
            DropTable("dbo.PremierPredictions");
            DropTable("dbo.PremierFixtures");
            DropTable("dbo.ChampionPredictions");
            CreateIndex("dbo.Predictions", "FixtureId");
            AddForeignKey("dbo.Predictions", "FixtureId", "dbo.Fixtures", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UserChampionScore4", newName: "UserScores");
            RenameTable(name: "dbo.ChampionFixtures", newName: "Fixtures");
        }
    }
}
