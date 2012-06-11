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
                                      DinnerID = int.Parse(dinner.Id.Substring(dinner.Id.LastIndexOf("/") + 1)),
                                      Title = dinner.Title,
                                      Latitude = dinner.Latitude,
                                      Longitude = dinner.Longitude,
                                      Description = dinner.Description,
                                      Url = dinner.Id.Substring(dinner.Id.LastIndexOf("/") + 1),
                                      EventDate = dinner.EventDate,
                                      RSVPCount = rsvpCount,
                                  };

            this.Store(x => x.DinnerID, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.Title, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.Latitude, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.Longitude, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.Description, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.Url, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.EventDate, Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store(x => x.RSVPCount, Raven.Abstractions.Indexing.FieldStorage.Yes);

            this.Index(dinner => dinner.RSVPCount, Raven.Abstractions.Indexing.FieldIndexing.NotAnalyzed);

            this.Sort(dinner => dinner.RSVPCount, Raven.Abstractions.Indexing.SortOptions.Int); 
        }
    }
}