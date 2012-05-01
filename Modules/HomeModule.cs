using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using DinnerParty.Models;


namespace DinnerParty.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                base.Page.Title = "Home";

                return View["Index", base.Model];
            };

            Get["/about"] = parameters =>
            {
                throw new Exception("Oops");
                base.Page.Title = "About";

                return View["About", base.Model];
            };

           

        }

    }
}