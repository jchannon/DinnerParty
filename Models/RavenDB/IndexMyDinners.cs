using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    public class IndexMyDinners : AbstractIndexCreationTask<Dinner>
    {
       
        public IndexMyDinners()
        {
            this.Map = dinners => from dinner in dinners
                                  from rsvp in dinner.RSVPs
                                  select new
                                  {
                                      RSVPs_AttendeeName = rsvp.AttendeeName,
                                      RSVPs_AttendeeNameId = rsvp.AttendeeNameId,
                                      HostedById = dinner.HostedById,
                                      EventDate = dinner.EventDate,
                                      HostedBy = dinner.HostedBy,
                                  };
        }


    }
}