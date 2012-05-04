using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    public class IndexEventDate : AbstractIndexCreationTask<Dinner>
    {
        public IndexEventDate()
        {
            this.Map = dinners =>
                                from dinner in dinners
                                select new
                                {
                                    EventDate = dinner.EventDate
                                };
        }
    }
}