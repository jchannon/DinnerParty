using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnerParty.Models
{
    public class PageModel
    {
        public string PreFixTitle { get; set; }
        public string Title { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}