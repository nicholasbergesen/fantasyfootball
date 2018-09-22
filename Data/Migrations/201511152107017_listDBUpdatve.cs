namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class listDBUpdatve : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fixtures", newName: "ChampionFixtures");
            RenameTable(name: "dbo.UserScores", newName: "UserChampionScores");
            RenameTable(name: "dbo.Predictions", newName: "ChampionPredictions");
            DropIndex("dbo.Predictions", new[] { "FixtureId" });
            RenameColumn(table: "dbo.ChampionPredictions", name: "FixtureId", newName: "ChampionFixtureID");
            RenameColumn(table: "dbo.PremierPredictions", name: "FixtureId", newName: "PremierFixtureID");
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
            
            CreateIndex("dbo.ChampionPredictions", "ChampionFixtureID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PremierPredictions", new[] { "PremierFixtureID" });
            DropIndex("dbo.ChampionPredictions", new[] { "ChampionFixtureID" });
            DropTable("dbo.UserPremierScores");
            DropTable("dbo.PremierPredictions");
            DropTable("dbo.PremierFixtures");
            RenameColumn(table: "dbo.PremierPredictions", name: "PremierFixtureID", newName: "FixtureId");
            RenameColumn(table: "dbo.ChampionPredictions", name: "ChampionFixtureID", newName: "FixtureId");
            CreateIndex("dbo.Predictions", "FixtureId");
            RenameTable(name: "dbo.ChampionPredictions", newName: "Predictions");
            RenameTable(name: "dbo.UserChampionScores", newName: "UserScores");
            RenameTable(name: "dbo.ChampionFixtures", newName: "Fixtures");
        }
    }
}
