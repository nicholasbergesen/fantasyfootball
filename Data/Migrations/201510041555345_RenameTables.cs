namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTables : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.UserScores", newName: "UserPremierScores");
            //RenameTable(name: "dbo.Predictions", newName: "PremierPredictions");
            //RenameTable(name: "dbo.Fixtures", newName: "PremierFixtures");
        }
        
        public override void Down()
        {
            //RenameTable(name: "UserPremierScores", newName: "dbo.UserScores");
            //RenameTable(name: "PremierPredictions", newName: "dbo.Predictions");
            //RenameTable(name: "PremierFixtures", newName: "dbo.Fixtures");
        }
    }
}
