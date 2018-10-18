using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data.Models;
using System.Data;
using WebMatrix.WebData;
using Data;

namespace FantasyFootball.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        FantasyFootballDb Db = new FantasyFootballDb();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if(!string.IsNullOrEmpty(HttpContext.User.Identity.Name))
                return RedirectToLocal(returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                using (FantasyFootballDb Db = new FantasyFootballDb())
                {
                    var userProfile = Db.UserProfiles.FirstOrDefault(x => model.UserName.Equals(x.UserName, StringComparison.InvariantCultureIgnoreCase));
                    if (userProfile != null)
                    {
                        userProfile.LoginCount++;
                        Db.SaveChanges();
                    }
                }

                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (WebSecurity.UserExists(model.UserName))
                        return RedirectToAction("Login", "Account");

                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    using (FantasyFootballDb db = new FantasyFootballDb())
                    {
                        UserProfile userProfile = new UserProfile { UserName = model.UserName };
                        UserPremierScore premierScore = new UserPremierScore { Score = 0, UserName = model.UserName };
                        UserChampionScore champtionScore = new UserChampionScore { Score = 0, UserName = model.UserName };
                        UserEuroScore euroScore = new UserEuroScore { Score = 0, UserName = model.UserName };
                        db.UserPremierScores.Add(premierScore);
                        db.UserChampionScores.Add(champtionScore);
                        db.UserEuroScores.Add(euroScore);
                        db.UserProfiles.Add(userProfile);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "PremierFixture");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = true;
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ManageTeams()
        {
            var teams = Db.Team.OrderBy(x => x.LeagueID);
            ViewBag.Leagues = Db.League;
            return View(teams);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageTeams(Team team)
        {
            Db.Team.Add(team);
            Db.SaveChanges();

            var teams = Db.Team.OrderBy(x => x.LeagueID);
            ViewBag.Leagues = Db.League;

            return View(teams);
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var team = Db.Team.FirstOrDefault(x => x.ID == id);
            Db.Team.Remove(team);
            Db.SaveChanges();

            var teams = Db.Team.OrderBy(x => x.LeagueID);
            ViewBag.Leagues = Db.League;

            return RedirectToAction("ManageTeams");
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "PremierFixture");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
