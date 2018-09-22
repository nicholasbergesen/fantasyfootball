namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFixtureIDs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChampionPredictions", "ChampionFixture_Id", "dbo.ChampionFixtures");
            DropForeignKey("dbo.PremierPredictions", "PremierFixture_Id", "dbo.PremierFixtures");
            DropIndex("dbo.ChampionPredictions", new[] { "ChampionFixture_Id" });
            DropIndex("dbo.PremierPredictions", new[] { "PremierFixture_Id" });
            RenameColumn(table: "dbo.ChampionPredictions", name: "ChampionFixture_Id", newName: "ChampionFixtureId");
            RenameColumn(table: "dbo.PremierPredictions", name: "PremierFixture_Id", newName: "PremierFixtureId");
            AlterColumn("dbo.ChampionPredictions", "ChampionFixtureId", c => c.Int(nullable: false));
            AlterColumn("dbo.PremierPredictions", "PremierFixtureId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChampionPredictions", "ChampionFixtureId");
            CreateIndex("dbo.PremierPredictions", "PremierFixtureId");
            AddForeignKey("dbo.ChampionPredictions", "ChampionFixtureId", "dbo.ChampionFixtures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PremierPredictions", "PremierFixtureId", "dbo.PremierFixtures", "Id", cascadeDelete: true);
            DropColumn("dbo.ChampionPredictions", "FixtureId");
            DropColumn("dbo.PremierPredictions", "FixtureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PremierPredictions", "FixtureId", c => c.Int(nullable: false));
            AddColumn("dbo.ChampionPredictions", "FixtureId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PremierPredictions", "PremierFixtureId", "dbo.PremierFixtures");
            DropForeignKey("dbo.ChampionPredictions", "ChampionFixtureId", "dbo.ChampionFixtures");
            DropIndex("dbo.PremierPredictions", new[] { "PremierFixtureId" });
            DropIndex("dbo.ChampionPredictions", new[] { "ChampionFixtureId" });
            AlterColumn("dbo.PremierPredictions", "PremierFixtureId", c => c.Int());
            AlterColumn("dbo.ChampionPredictions", "ChampionFixtureId", c => c.Int());
            RenameColumn(table: "dbo.PremierPredictions", name: "PremierFixtureId", newName: "PremierFixture_Id");
            RenameColumn(table: "dbo.ChampionPredictions", name: "ChampionFixtureId", newName: "ChampionFixture_Id");
            CreateIndex("dbo.PremierPredictions", "PremierFixture_Id");
            CreateIndex("dbo.ChampionPredictions", "ChampionFixture_Id");
            AddForeignKey("dbo.PremierPredictions", "PremierFixture_Id", "dbo.PremierFixtures", "Id");
            AddForeignKey("dbo.ChampionPredictions", "ChampionFixture_Id", "dbo.ChampionFixtures", "Id");
        }
    }
}
