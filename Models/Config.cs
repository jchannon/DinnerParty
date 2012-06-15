using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DinnerParty.Models
{
    public class Config
    {
        public string Id { get; set; }

        public DateTime LastTruncateDate { get; set; }
    }
}