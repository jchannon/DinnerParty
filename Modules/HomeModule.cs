using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using DinnerParty.Models;
using Nancy.Authentication.Forms;

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

            Get["/About"] = parameters =>
            {
                base.Page.Title = "About";

                return View["About", base.Model];
            };

        }

    }
}