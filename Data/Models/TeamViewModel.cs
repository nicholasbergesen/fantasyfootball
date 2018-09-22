using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Data.Models
{
    public class TeamViewModel
    {
        public List<Team> Teams { get; set; }
        public List<League> Leagues { get; set; }

        public IEnumerable<SelectListItem> SelectLeagues
        {
            get
            {
                var allLeagues = Leagues.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.LeagueName
                });
                return allLeagues;

            }
        }
    }
}
