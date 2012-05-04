using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    public class IndexMyDinners : AbstractIndexCreationTask<Dinner>
    {
        public class MyDinner
        {
            public string AttendeeName { get; set; }
            public string AttendeeNameId { get; set; }
            public string HostedById { get; set; }
            public DateTime EventDate { get; set; }
            public string HostedBy { get; set; }
        }
        public IndexMyDinners()
        {
            this.Map = dinners => from dinner in dinners
                                  from rsvp in dinner.RSVPs//.DefaultIfEmpty()
                                  select new
                                  {
                                      RSVPs_AttendeeName = rsvp.AttendeeName,
                                      RSVPs_AttendeeNameId = rsvp.AttendeeNameId,
                                      HostedById = dinner.HostedById,
                                      EventDate = dinner.EventDate,
                                      HostedBy = dinner.HostedBy,
                                  };

        }

        //          from doc in docs.Dinners
        //          from docRSVPsItem in ((IEnumerable<dynamic>)doc.RSVPs).DefaultIfEmpty()
        //          select new { RSVPs_AttendeeNameId = docRSVPsItem.AttendeeNameId, RSVPs_AttendeeName = docRSVPsItem.AttendeeName, 
        //                       HostedById = doc.HostedById, EventDate = doc.EventDate, HostedBy = doc.HostedBy }
    }
}