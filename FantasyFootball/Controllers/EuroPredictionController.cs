using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FantasyFootball.Controllers
{
    public class EuroPredictionController : Controller
    {
        FantasyFootballDb db = new FantasyFootballDb();

        [Authorize]
        public ActionResult Index([Bind(Prefix = "Id")]int FixtureId)
        {
            var UserPostedList = db.EuroPredictions.Select(item => item).
                Where(item => item.EuroFixtureID == FixtureId
                    && item.UserName == User.Identity.Name).ToList();

            ViewData["HasUserPosted"] = UserPostedList.Count;
            var Fixture = db.EuroFixtures.Find(FixtureId);
            return View(Fixture);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(int FixtureId)
        {
            ViewData["FixID"] = FixtureId;
            ViewData["Home"] = (string)db.EuroFixtures.Find(FixtureId).HomeTeam;
            ViewData["Away"] = (string)db.EuroFixtures.Find(FixtureId).AwayTeam;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(EuroPrediction prediction, int FixtureId)
        {
            prediction.EuroFixtureID = FixtureId;
            prediction.UserName = User.Identity.Name;

            if (HasUserAlreadyPosted(prediction))
            {
                return RedirectToAction("Index", new { Id = prediction.EuroFixtureID });
            }
            else if (ModelState.IsValid)
            {
                prediction.DatePosted = DateTime.Now;
                db.EuroPredictions.Add(prediction);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = prediction.EuroFixtureID });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCreate(int FixtureId)
        {
            var userNames = db.UserProfiles.Select(x => x.UserName);
            ViewData["UserList"] = new SelectList(userNames);

            ViewData["FixID"] = FixtureId;
            var fixture = db.EuroFixtures.Where(x => x.Id.Equals(FixtureId)).FirstOrDefault();

            ViewData["Home"] = fixture.HomeTeam;
            ViewData["Away"] = fixture.AwayTeam;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCreate(EuroPrediction prediction, int FixtureId)
        {
            prediction.EuroFixtureID = FixtureId;

            if (HasUserAlreadyPosted(prediction))
            {
                return RedirectToAction("Index", new { Id = prediction.EuroFixtureID });
            }
            else if (ModelState.IsValid)
            {
                prediction.DatePosted = DateTime.Now;
                db.EuroPredictions.Add(prediction);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = prediction.EuroFixtureID });
        }

        private bool HasUserAlreadyPosted(EuroPrediction prediction)
        {
            return db.EuroPredictions.FirstOrDefault(x => x.UserName.Equals(prediction.UserName) && x.EuroFixtureID.Equals(prediction.EuroFixtureID)) != null;
        }

    }
}
