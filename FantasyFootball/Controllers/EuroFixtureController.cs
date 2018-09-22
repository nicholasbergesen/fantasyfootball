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
    public class EuroFixtureController : Controller
    {
        private FantasyFootballDb db = new FantasyFootballDb();

        [Authorize]
        public ActionResult Index()
        {
            db.UserProfiles.Single(x => x.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).FixtureCount++;
            db.SaveChanges();
            return View(db.EuroFixtures.OrderByDescending(x => x.FixtureDate).Take(15).ToList());
        }

        [Authorize]
        public ActionResult FullList()
        {
            return View(db.EuroFixtures.OrderByDescending(x => x.FixtureDate).ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            var teams = db.Team.Where(x => x.LeagueID == 2).Select(x => x.Description);

            ViewData["TeamList"] = new SelectList(teams);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(EuroFixture fixture)
        {
            if (ModelState.IsValid)
            {
                fixture.HomeScore = -1;
                fixture.AwayScore = -1;
                db.EuroFixtures.Add(fixture);
                db.Entry(fixture).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            EuroFixture fixture = db.EuroFixtures.FirstOrDefault(x => x.Id == id);

            if (fixture == null)
                return HttpNotFound();

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(EuroFixture fixture)
        {
            EuroFixture newfix = db.EuroFixtures.Find(fixture.Id);
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

        private void CalculateUserPoints(EuroFixture fixture)
        {
            var predictions = fixture.EuroPedictions;
            List<EuroPrediction> exactUserPredictions = new List<EuroPrediction>();
            List<EuroPrediction> partialUserPredictions = new List<EuroPrediction>();
            foreach (var prediction in predictions)
            {
                if (prediction.HomePrediction == fixture.HomeScore && prediction.AwayPrediction == fixture.AwayScore)
                    exactUserPredictions.Add(prediction);
                else
                    partialUserPredictions.Add(prediction);
            }

            if (exactUserPredictions.Count == 1)
            {
                EuroPrediction firstPrediction = exactUserPredictions.FirstOrDefault();
                UserEuroScore userScore = db.UserEuroScores.FirstOrDefault(x => x.UserName == firstPrediction.UserName);
                if (DateTime.Parse("2016/6/24 12:00:00 AM") < fixture.FixtureDate)
                    AddPointsToUser(userScore, 6, fixture);
                else
                    AddPointsToUser(userScore, 4, fixture);
            }
            else if (exactUserPredictions.Count > 1)
            {
                foreach (var prediction in exactUserPredictions)
                {
                    UserEuroScore userScore = db.UserEuroScores.FirstOrDefault(x => x.UserName == prediction.UserName);
                    if (DateTime.Parse("2016/6/24 12:00:00 AM") < fixture.FixtureDate)
                        AddPointsToUser(userScore, 5, fixture);
                    else
                        AddPointsToUser(userScore, 3, fixture);
                }
            }

            if (partialUserPredictions.Count > 0)
            {
                foreach (var prediction in partialUserPredictions)
                {
                    if (IsResultCorrect(prediction, fixture))
                    {
                        UserEuroScore userScore = db.UserEuroScores.FirstOrDefault(x => x.UserName == prediction.UserName);
                        if (DateTime.Parse("2016/6/24 12:00:00 AM") < fixture.FixtureDate)
                            AddPointsToUser(userScore, 2, fixture);
                        else
                            AddPointsToUser(userScore, 1, fixture);
                    }
                }
            }

            db.SaveChanges();
        }

        private bool IsResultCorrect(EuroPrediction prediction, EuroFixture fixture)
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

        private void AddPointsToUser(UserEuroScore userScore, int score, EuroFixture fixture)
        {
            if ((DateTime.Parse("2016/6/24 12:00:00 AM") < fixture.FixtureDate) 
                && ((score == 6) || (score == 5)))
                    userScore.ExactPredictions += 1;
            else if (!(DateTime.Parse("2016/6/24 12:00:00 AM") < fixture.FixtureDate) 
                && (score == 4) || (score == 3))
                userScore.ExactPredictions += 1;

            userScore.Score += score;
            db.Entry<UserEuroScore>(userScore).State = EntityState.Modified;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            EuroFixture fixture = db.EuroFixtures.FirstOrDefault(x => x.Id == id);

            if (fixture == null)
                return HttpNotFound();

            return View(fixture);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EuroFixture fixture = db.EuroFixtures.FirstOrDefault(x => x.Id == id);
            db.EuroFixtures.Remove(fixture);
            db.SaveChanges();
            ReCalculateScores();
            return RedirectToAction("Index");
        }

        private void ReCalculateScores()
        {
            List<UserEuroScore> users = db.UserEuroScores.Select(x => x).ToList();
            foreach (var user in users)
            {
                user.ExactPredictions = 0;
                user.Score = 0;
                db.Entry<UserEuroScore>(user).State = EntityState.Modified;
            }

            List<EuroFixture> fixtures = db.EuroFixtures.Where(x => x.AwayScore != -1 && x.HomeScore != -1).ToList();
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
