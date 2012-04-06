using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using System.Dynamic;
using DinnerParty.Models;

namespace DinnerParty.Modules
{
    public class BaseModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();
        protected PageModel Page { get; set; }

        public BaseModule()
        {
            SetupModelDefaults();
        }

        public BaseModule(string modulepath)
            : base(modulepath)
        {
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                Page = new PageModel()
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    PreFixTitle = "Dinner Party - "
                };

                Model.Page = Page;

                return null;
            };

        }
    }
}