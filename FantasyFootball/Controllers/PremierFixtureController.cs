using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Models;
using Data;

namespace FantasyFootball.Controllers
{
    public class PremierFixtureController : Controller
    {
        private FantasyFootballDb db = new FantasyFootballDb();

        [Authorize]
        public ActionResult Index()
        {
            db.UserProfiles.Single(x => x.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).FixtureCount++;
            db.SaveChanges();
            return View(db.PremierFixtures.OrderByDescending(x => x.FixtureDate).Take(15).ToList());
        }

        [Authorize]
        public ActionResult FullList()
        {
            return View(db.PremierFixtures.OrderByDescending(x => x.FixtureDate).ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            var teams = db.Team.Where(x => x.LeagueID == 3).Select(x => x.Description);

            ViewData["TeamList"] = new SelectList(teams);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(PremierFixture fixture)
        {
            if (ModelState.IsValid)
            {
                fixture.HomeScore = -1;
                fixture.AwayScore = -1;
                db.PremierFixtures.Add(fixture);
                db.Entry(fixture).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fixture);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id = 0)
        {
            PremierFixture fixture = db.PremierFixtures.FirstOrDefault(x => x.Id == id);

            if (fixture == null)
                return HttpNotFound();

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(PremierFixture fixture)
        {
            PremierFixture newfix = db.PremierFixtures.Find(fixture.Id);
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

        private void CalculateUserPoints(PremierFixture fixture)
        {
            var predictions = fixture.PremierPedictions;
            List<PremierPrediction> exactUserPredictions = new List<PremierPrediction>();
            List<PremierPrediction> partialUserPredictions = new List<PremierPrediction>();
            foreach (var prediction in predictions)
	        {
                if (prediction.HomePrediction == fixture.HomeScore && prediction.AwayPrediction == fixture.AwayScore)
                    exactUserPredictions.Add(prediction);
                else
                    partialUserPredictions.Add(prediction);
	        }
            
            if (exactUserPredictions.Count == 1)
            {
                PremierPrediction firstPrediction = exactUserPredictions.FirstOrDefault();
                UserPremierScore userScore = db.UserPremierScores.FirstOrDefault(x => x.UserName == firstPrediction.UserName);
                AddPointsToUser(userScore, 4);
            }
            else if (exactUserPredictions.Count > 1)
            {
                foreach (var prediction in exactUserPredictions)
                {
                    UserPremierScore userScore = db.UserPremierScores.FirstOrDefault(x => x.UserName == prediction.UserName);
                    AddPointsToUser(userScore, 3);
                }
            }
            
            if (partialUserPredictions.Count > 0)
            {
                foreach (var prediction in partialUserPredictions)
                {
                    if (IsResultCorrect(prediction, fixture))
                    {
                        UserPremierScore userScore = db.UserPremierScores.FirstOrDefault(x => x.UserName == prediction.UserName);
                        if(userScore != null)
                            AddPointsToUser(userScore, 1);
                    }
                }
            }

            db.SaveChanges();
        }

        private bool IsResultCorrect(PremierPrediction prediction, PremierFixture fixture)
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

        private void AddPointsToUser(UserPremierScore userScore, int score)
        {
            userScore.Score += score;
            if ((score == 4) || (score == 3))
                userScore.ExactPredictions += 1;
            db.Entry<UserPremierScore>(userScore).State = EntityState.Modified;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            PremierFixture fixture = db.PremierFixtures.FirstOrDefault(x => x.Id == id);

            if (fixture == null)
                return HttpNotFound();

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PremierFixture fixture = db.PremierFixtures.FirstOrDefault(x => x.Id == id);
            db.PremierFixtures.Remove(fixture);
            db.SaveChanges();
            ReCalculateScores();
            return RedirectToAction("Index");
        }

        private void ReCalculateScores()
        {
            List<UserPremierScore> users = db.UserPremierScores.Select(x => x).ToList();
            foreach (var user in users)
            {
                user.ExactPredictions = 0;
                user.Score = 0;
                db.Entry<UserPremierScore>(user).State = EntityState.Modified;
            }

            List<PremierFixture> fixtures = db.PremierFixtures.Where(x => x.AwayScore != -1 && x.HomeScore != -1).ToList();
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