namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeagueTeamRelationShip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "LeagueID", c => c.Int(nullable: false));
            CreateIndex("dbo.Teams", "LeagueID");
            AddForeignKey("dbo.Teams", "LeagueID", "dbo.Leagues", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "LeagueID", "dbo.Leagues");
            DropIndex("dbo.Teams", new[] { "LeagueID" });
            DropColumn("dbo.Teams", "LeagueID");
        }
    }
}
