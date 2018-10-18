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

            db.SaveChanges();
        }
    }
}