using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DinnerParty.Models;
using Nancy;
using DinnerParty.Models.RavenDB;

namespace DinnerParty.Modules
{
    public class ServicesModule : PersistModule
    {
        public ServicesModule()
            : base("/services")
        {
            Get["/RSS"] = parameters =>
                {
                    var dinners = DocumentSession.Query<Dinner, Dinners_Index>().Where(d => d.EventDate > DateTime.Now.Date).OrderBy(x => x.EventDate).AsEnumerable();

                    if (dinners == null)
                    {
                        base.Page.Title = "Nerd Dinner Not Found";
                        return View["NotFound", base.Model];
                    }

                    return this.Response.AsRSS(dinners, "Upcoming Nerd Dinners");
                };
        }
    }
}