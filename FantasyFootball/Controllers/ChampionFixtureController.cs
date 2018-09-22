using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FantasyFootball.Controllers
{
    public class ChampionFixtureController : Controller
    {
        private FantasyFootballDb db = new FantasyFootballDb();

        [Authorize]
        public ActionResult Index()
        {
            db.UserProfiles.Single(x => x.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).FixtureCount++;
            db.SaveChanges();
            return View(db.ChampionFixtures.OrderByDescending(x => x.FixtureDate).Take(15).ToList());
        }

        [Authorize]
        public ActionResult FullList()
        {
            return View(db.ChampionFixtures.OrderByDescending(x => x.FixtureDate).ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            var teams = db.Team.Where(x => x.LeagueID == 1).Select(x => x.Description);

            ViewData["TeamList"] = new SelectList(teams);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ChampionFixture fixture)
        {
            if (ModelState.IsValid)
            {
                fixture.HomeScore = -1;
                fixture.AwayScore = -1;
                db.ChampionFixtures.Add(fixture);
                db.Entry(fixture).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            ChampionFixture fixture = db.ChampionFixtures.FirstOrDefault(x => x.Id == id);

            if (fixture == null)
                return HttpNotFound();

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(ChampionFixture fixture)
        {
            ChampionFixture newfix = db.ChampionFixtures.Find(fixture.Id);
            newfix.HomeScore = fixture.HomeScore;
            newfix.AwayScore = fixture.AwayScore;
            db.SaveChanges();

            if (ModelState.IsValid)
            {
                CalculateUserPoints(newfix);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Recalculate()
        {
            ReCalculateScores();
            return View();
        }

        private void CalculateUserPoints(ChampionFixture fixture)
        {
            var predictions = fixture.ChampionPedictions;
            List<ChampionPrediction> exactUserPredictions = new List<ChampionPrediction>();
            List<ChampionPrediction> partialUserPredictions = new List<ChampionPrediction>();
            foreach (var prediction in predictions)
            {
                if (prediction.HomePrediction == fixture.HomeScore && prediction.AwayPrediction == fixture.AwayScore)
                    exactUserPredictions.Add(prediction);
                else
                    partialUserPredictions.Add(prediction);
            }

            if (exactUserPredictions.Count == 1)
            {
                ChampionPrediction firstPrediction = exactUserPredictions.FirstOrDefault();
                UserChampionScore userScore = db.UserChampionScores.FirstOrDefault(x => x.UserName == firstPrediction.UserName);
                AddPointsToUser(userScore, 4);
            }
            else if (exactUserPredictions.Count > 1)
            {
                foreach (var prediction in exactUserPredictions)
                {
                    UserChampionScore userScore = db.UserChampionScores.FirstOrDefault(x => x.UserName == prediction.UserName);
                    AddPointsToUser(userScore, 3);
                }
            }

            if (partialUserPredictions.Count > 0)
            {
                foreach (var prediction in partialUserPredictions)
                {
                    if (IsResultCorrect(prediction, fixture))
                    {
                        UserChampionScore userScore = db.UserChampionScores.FirstOrDefault(x => x.UserName == prediction.UserName);
                        AddPointsToUser(userScore, 1);
                    }
                }
            }

            db.SaveChanges();
        }

        private bool IsResultCorrect(ChampionPrediction prediction, ChampionFixture fixture)
        {
            return ((prediction.HomePrediction < prediction.AwayPrediction
                    && fixture.HomeScore < fixture.AwayScore)
                    ||
                    (prediction.HomePrediction > prediction.AwayPrediction
                    && fixture.HomeScore > fixture.AwayScore)
                    ||
                    (prediction.HomePrediction == prediction.AwayPrediction
                    && fixture.HomeScore == fixture.AwayScore));
        }

        private void AddPointsToUser(UserChampionScore userScore, int score)
        {
            userScore.Score += score;
            if ((score == 4) || (score == 3))
                userScore.ExactPredictions += 1;
            db.Entry<UserChampionScore>(userScore).State = EntityState.Modified;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            ChampionFixture fixture = db.ChampionFixtures.FirstOrDefault(x => x.Id == id);

            if (fixture == null)
                return HttpNotFound();

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ChampionFixture fixture = db.ChampionFixtures.FirstOrDefault(x => x.Id == id);
            db.ChampionFixtures.Remove(fixture);
            db.SaveChanges();
            ReCalculateScores();
            return RedirectToAction("Index");
        }

        private void ReCalculateScores()
        {
            List<UserChampionScore> users = db.UserChampionScores.Select(x => x).ToList();
            foreach (var user in users)
            {
                user.ExactPredictions = 0;
                user.Score = 0;
                db.Entry<UserChampionScore>(user).State = EntityState.Modified;
            }

            List<ChampionFixture> fixtures = db.ChampionFixtures.Where(x => x.AwayScore != -1 && x.HomeScore != -1).ToList();
            foreach (var fixture in fixtures)
            {
                CalculateUserPoints(fixture);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
 
    }
}
