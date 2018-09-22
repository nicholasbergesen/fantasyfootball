namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChampionFixtures",
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
                "dbo.EuroFixtures",
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
                "dbo.EuroPredictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePosted = c.DateTime(nullable: false),
                        HomePrediction = c.Int(nullable: false),
                        AwayPrediction = c.Int(nullable: false),
                        EuroFixtureID = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EuroFixtures", t => t.EuroFixtureID, cascadeDelete: true)
                .Index(t => t.EuroFixtureID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DatePosted = c.DateTime(nullable: false),
                        Comment = c.String(),
                        PostedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.UserChampionScores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Score = c.Int(nullable: false),
                        ExactPredictions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserEuroScores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Score = c.Int(nullable: false),
                        ExactPredictions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FixtureCount = c.Int(nullable: false),
                        ScoreCount = c.Int(nullable: false),
                        PostCount = c.Int(nullable: false),
                        LoginCount = c.Int(nullable: false),
                        FunCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PremierPredictions", "PremierFixtureID", "dbo.PremierFixtures");
            DropForeignKey("dbo.EuroPredictions", "EuroFixtureID", "dbo.EuroFixtures");
            DropForeignKey("dbo.ChampionPredictions", "ChampionFixtureID", "dbo.ChampionFixtures");
            DropIndex("dbo.PremierPredictions", new[] { "PremierFixtureID" });
            DropIndex("dbo.EuroPredictions", new[] { "EuroFixtureID" });
            DropIndex("dbo.ChampionPredictions", new[] { "ChampionFixtureID" });
            DropTable("dbo.UserProfiles");
            DropTable("dbo.UserPremierScores");
            DropTable("dbo.UserEuroScores");
            DropTable("dbo.UserChampionScores");
            DropTable("dbo.PremierPredictions");
            DropTable("dbo.PremierFixtures");
            DropTable("dbo.Posts");
            DropTable("dbo.EuroPredictions");
            DropTable("dbo.EuroFixtures");
            DropTable("dbo.ChampionPredictions");
            DropTable("dbo.ChampionFixtures");
        }
    }
}
