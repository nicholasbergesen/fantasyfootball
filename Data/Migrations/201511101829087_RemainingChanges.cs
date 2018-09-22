namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemainingChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PremierPredictions", new[] { "ChampionFixture_Id" });
            AddColumn("dbo.ChampionPredictions", "ChampionFixture_Id", c => c.Int());
            CreateIndex("dbo.ChampionPredictions", "ChampionFixture_Id");
            DropColumn("dbo.PremierPredictions", "ChampionFixture_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PremierPredictions", "ChampionFixture_Id", c => c.Int());
            DropIndex("dbo.ChampionPredictions", new[] { "ChampionFixture_Id" });
            DropColumn("dbo.ChampionPredictions", "ChampionFixture_Id");
            CreateIndex("dbo.PremierPredictions", "ChampionFixture_Id");
        }
    }
}
