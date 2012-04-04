using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using DinnerParty.Models;

namespace DinnerParty.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                var model = new Home() { Header = "Welcome to DinnerParty", Body = "A Nerddinner port" };

                return View["Index", model];
            };
        }
    }
}