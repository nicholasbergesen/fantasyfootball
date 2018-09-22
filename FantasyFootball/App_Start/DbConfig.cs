using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Data.Models;
using WebMatrix.WebData;
using Data;

namespace FantasyFootball
{
    public class DbConfig
    {
        public static void RegisterDb()
        {
            FantasyFootballDb db = new FantasyFootballDb();
            db.Database.CreateIfNotExists();

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "WebUsers", "UserId", "UserName", autoCreateTables: true);
            
            if(!Roles.RoleExists("Admin"))
                Roles.CreateRole("Admin");
            if (!WebSecurity.UserExists("Bergie"))
            {
                WebSecurity.CreateUserAndAccount("Bergie", "910313");
                db.UserProfiles.Add(new UserProfile() { UserName = "Bergie" });
                db.UserPremierScores.Add(new UserPremierScore() { Score = 0, UserName = "Bergie" });
            }
            if(!Roles.IsUserInRole("Bergie","Admin"))
                Roles.AddUserToRole("Bergie", "Admin");

            db.SaveChanges();
        }
    }
}