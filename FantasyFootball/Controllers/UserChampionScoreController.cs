using Data;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FantasyFootball.Controllers
{
    public class UserChampionScoreController : Controller
    {
        private FantasyFootballDb db = new FantasyFootballDb();

        public ActionResult Index()
        {
            db.UserProfiles.Single(x => x.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).ScoreCount++;
            db.SaveChanges();
            var log = db.UserChampionScores.OrderByDescending(x => x.Score);
            return View(log);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
