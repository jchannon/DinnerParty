using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    public class Dinners_Index : AbstractIndexCreationTask<Dinner>
    {
        public Dinners_Index()
        {
            this.Map = dinners =>
                       from dinner in dinners
                       select new
                       {
                           RSVPs_AttendeeName = dinner.RSVPs.Select(x => x.AttendeeName),
                           RSVPs_AttendeeNameId = dinner.RSVPs.Select(x => x.AttendeeNameId),
                           HostedById = dinner.HostedById,
                           HostedBy = dinner.HostedBy,
                           DinnerID = int.Parse(dinner.Id.Substring(dinner.Id.LastIndexOf("/") + 1)),
                           Title = dinner.Title,
                           Latitude = dinner.Latitude,
                           Longitude = dinner.Longitude,
                           Description = dinner.Description,
                           EventDate = dinner.EventDate,
                           RSVPCount = dinner.RSVPs.Count,
                           Url = dinner.Id.Substring(dinner.Id.LastIndexOf("/") + 1)
                       };

            this.Store("Url", Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store("RSVPCount", Raven.Abstractions.Indexing.FieldStorage.Yes);
            this.Store("DinnerID", Raven.Abstractions.Indexing.FieldStorage.Yes);

            this.Index("RSVPCount", Raven.Abstractions.Indexing.FieldIndexing.NotAnalyzed);
        }
    }
}