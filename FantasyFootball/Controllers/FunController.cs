using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Models;
using Data;

namespace FantasyFootball.Controllers
{
    public class FunController : Controller
    {
        public ActionResult Index()
        {
            using (FantasyFootballDb db = new FantasyFootballDb())
            {
                db.UserProfiles.Single(x => x.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).FunCount++;
                db.SaveChanges();
            }
            return View();
        }
    }
}
