namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateChampionTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fixtures", newName: "PremierFixtures");
            RenameTable(name: "dbo.UserScores", newName: "UserPremierScores");
            RenameTable(name: "dbo.Predictions", newName: "PremierPredictions");
            DropIndex("dbo.Predictions", new[] { "FixtureId" });
            RenameColumn(table: "dbo.PremierPredictions", name: "FixtureId", newName: "PremierFixtureID");
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
                        ChampionsFixtureID = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ChampionsFixtureID);
            
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
            AddForeignKey("dbo.ChampionPredictions", "ChampionFixtureId", "dbo.ChampionFixtures", "Id", cascadeDelete: true);
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
