namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEuroTables : DbMigration
    {
        public override void Up()
        {
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
                "dbo.UserEuroScores",
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
            DropForeignKey("dbo.EuroPredictions", "EuroFixtureID", "dbo.EuroFixtures");
            DropIndex("dbo.EuroPredictions", new[] { "EuroFixtureID" });
            DropTable("dbo.UserEuroScores");
            DropTable("dbo.EuroPredictions");
            DropTable("dbo.EuroFixtures");
        }
    }
}
