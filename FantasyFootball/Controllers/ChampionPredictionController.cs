using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FantasyFootball.Controllers
{
    public class ChampionPredictionController : Controller
    {
        FantasyFootballDb db = new FantasyFootballDb();

        [Authorize]
        public ActionResult Index([Bind(Prefix = "Id")]int FixtureId)
        {
            var UserPostedList = db.ChampionPredictions.Select(item => item).
                Where(item => item.ChampionFixtureID == FixtureId
                    && item.UserName == User.Identity.Name).ToList();

            ViewData["HasUserPosted"] = UserPostedList.Count;
            var Fixture = db.ChampionFixtures.Find(FixtureId);
            return View(Fixture);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(int FixtureId)
        {
            ViewData["FixID"] = FixtureId;
            ViewData["Home"] = (string)db.ChampionFixtures.Find(FixtureId).HomeTeam;
            ViewData["Away"] = (string)db.ChampionFixtures.Find(FixtureId).AwayTeam;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(ChampionPrediction prediction, int FixtureId)
        {
            prediction.ChampionFixtureID = FixtureId;
            prediction.UserName = User.Identity.Name;

            if (HasUserAlreadyPosted(prediction))
            {
                return RedirectToAction("Index", new { Id = prediction.ChampionFixtureID });
            }
            else if (ModelState.IsValid)
            {
                prediction.DatePosted = DateTime.Now;
                db.ChampionPredictions.Add(prediction);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = prediction.ChampionFixtureID });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCreate(int FixtureId)
        {
            var userNames = db.UserProfiles.Select(x => x.UserName);
            ViewData["UserList"] = new SelectList(userNames);

            ViewData["FixID"] = FixtureId;
            var fixture = db.ChampionFixtures.Where(x => x.Id.Equals(FixtureId)).FirstOrDefault();

            ViewData["Home"] = fixture.HomeTeam;
            ViewData["Away"] = fixture.AwayTeam;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCreate(ChampionPrediction prediction, int FixtureId)
        {
            prediction.ChampionFixtureID = FixtureId;

            if (HasUserAlreadyPosted(prediction))
            {
                return RedirectToAction("Index", new { Id = prediction.ChampionFixtureID });
            }
            else if (ModelState.IsValid)
            {
                prediction.DatePosted = DateTime.Now;
                db.ChampionPredictions.Add(prediction);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = prediction.ChampionFixtureID });
        }

        private bool HasUserAlreadyPosted(ChampionPrediction prediction)
        {
            return db.ChampionPredictions.FirstOrDefault(x => x.UserName.Equals(prediction.UserName) && x.ChampionFixtureID.Equals(prediction.ChampionFixtureID)) != null;
        }

    }
}
