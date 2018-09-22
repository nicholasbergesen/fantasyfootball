using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Data.Models;

namespace Data
{
    public class FantasyFootballDb : DbContext
    {
        public FantasyFootballDb()
            : base("name=DefaultConnection")
        {

        }


        public DbSet<PremierFixture> PremierFixtures { get; set; }
        public DbSet<PremierPrediction> PremierPredictions { get; set; }
        public DbSet<UserPremierScore> UserPremierScores { get; set; }

        public DbSet<ChampionFixture> ChampionFixtures { get; set; }
        public DbSet<ChampionPrediction> ChampionPredictions { get; set; }
        public DbSet<UserChampionScore> UserChampionScores { get; set; }

        public DbSet<EuroFixture> EuroFixtures { get; set; }
        public DbSet<EuroPrediction> EuroPredictions { get; set; }
        public DbSet<UserEuroScore> UserEuroScores { get; set; }

        public DbSet<League> League { get; set; }

        public DbSet<Team> Team { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}