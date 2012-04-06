using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace DinnerParty.Modules
{
    public class DinnerModule : BaseModule
    {
        public DinnerModule() : base("/dinners")
        {
            Get["/"] = parameters =>
            {
                return View["Index"];
            };
        }
    }
}