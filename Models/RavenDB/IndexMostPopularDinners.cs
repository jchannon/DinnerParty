using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    
    public class IndexMostPopularDinners : AbstractIndexCreationTask<Dinner, JsonDinner>
    {

        public IndexMostPopularDinners()
        {
            
            this.Map = dinners => from dinner in dinners
                                  let rsvpCount = dinner.RSVPs.Count()
                                  select new
                                  {
                                      EventDate = dinner.EventDate,

                                      RSVPCount = rsvpCount,
                                  };

            this.Index(dinner => dinner.RSVPCount, Raven.Abstractions.Indexing.FieldIndexing.NotAnalyzed);

            this.Sort(dinner => dinner.RSVPCount, Raven.Abstractions.Indexing.SortOptions.Int); 
        }
    }
}