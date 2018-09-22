using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Models;
using Data;

namespace FantasyFootball.Controllers
{
    public class PremierPredictionController : Controller
    {
        FantasyFootballDb db = new FantasyFootballDb();

        [Authorize]
        public ActionResult Index([Bind(Prefix="Id")]int FixtureId)
        {
            var UserPostedList = db.PremierPredictions.Select(item => item).
                Where(item => item.PremierFixtureID == FixtureId 
                    && item.UserName == User.Identity.Name).ToList();

            ViewData["HasUserPosted"] = UserPostedList.Count;
            var Fixture = db.PremierFixtures.Find(FixtureId);
            return View(Fixture);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(int FixtureId)
        {
            ViewData["FixID"] = FixtureId;
            ViewData["Home"] = (string)db.PremierFixtures.Find(FixtureId).HomeTeam;
            ViewData["Away"] = (string)db.PremierFixtures.Find(FixtureId).AwayTeam;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(PremierPrediction prediction, int FixtureId)
        {
            prediction.UserName = User.Identity.Name;
            prediction.PremierFixtureID = FixtureId;

            if (HasUserAlreadyPosted(prediction))
            {
                return RedirectToAction("Index", new { Id = prediction.PremierFixtureID });
            }
            else if (ModelState.IsValid)
            {
                prediction.DatePosted = DateTime.Now;
                db.PremierPredictions.Add(prediction);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = prediction.PremierFixtureID });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCreate(int FixtureId)
        {
            var userNames = db.UserProfiles.Select(x => x.UserName);
            ViewData["UserList"] = new SelectList(userNames);

            ViewData["FixID"] = FixtureId;
            var fixture = db.PremierFixtures.Where(x => x.Id.Equals(FixtureId)).FirstOrDefault();

            ViewData["Home"] = fixture.HomeTeam;
            ViewData["Away"] = fixture.AwayTeam;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCreate(PremierPrediction prediction, int FixtureId)
        {
            prediction.PremierFixtureID = FixtureId;

            if (HasUserAlreadyPosted(prediction))
            {
                return RedirectToAction("Index", new { Id = prediction.PremierFixtureID });
            }
            else if (ModelState.IsValid)
            {
                prediction.DatePosted = DateTime.Now;
                db.PremierPredictions.Add(prediction);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = prediction.PremierFixtureID });
        }

        private bool HasUserAlreadyPosted(PremierPrediction prediction)
        {
            return db.PremierPredictions.FirstOrDefault(x => x.UserName.Equals(prediction.UserName) &&  x.PremierFixtureID.Equals(prediction.PremierFixtureID)) != null;
        }
    }
}
