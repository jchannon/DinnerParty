using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DinnerParty.Models;
using Nancy;

namespace DinnerParty.Modules
{
    public class ServicesModule : PersistModule
    {
        public ServicesModule()
            : base("/services")
        {
            Get["/RSS"] = parameters =>
                {
                    var dinners = DocumentSession.Query<Dinner>().Where(d => d.EventDate > DateTime.UtcNow).OrderBy(x => x.EventDate).AsEnumerable();

                    if (dinners == null)
                        return View["NotFound"];
                    return this.Response.AsRSS(dinners, "Upcoming Nerd Dinners");
                };
        }
    }
}