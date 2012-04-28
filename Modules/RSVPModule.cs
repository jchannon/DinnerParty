using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using DinnerParty.Models;
using Nancy.RouteHelpers;

namespace DinnerParty.Modules
{
    public class RSVPModule : BaseModule
    {
        public RSVPModule()
            : base("/RSVP")
        {
            Get["/RsvpTwitterBegin/intcalledID"] = paramsd => { return "hi"; };

            Get["/RsvpBegin/intcalledID"] = paramsd =>
            {

                if (Request.Query.identifier.HasValue)
                {

                }

                return "hi";

            };


        }
    }

    public class RSVPAuthorizedModule : PersistModule
    {
        public RSVPAuthorizedModule() : base("/RSVP")
        {
            this.RequiresAuthentication();

            Post["/Cancel/" + Route.AnyIntOptional("id")] = parameters =>
            {
                Dinner dinner = DocumentSession.Load<Dinner>((int)parameters.id);

                RSVP rsvp = dinner.RSVPs
                    .Where(r => this.Context.CurrentUser.UserName == (r.AttendeeNameId ?? r.AttendeeName))
                    .SingleOrDefault();

                if (rsvp != null)
                {
                    dinner.RSVPs.Remove(rsvp);
                    DocumentSession.SaveChanges();

                }

                return "Sorry you can't make it!";
            };

            Post["/Register/" + Route.AnyIntOptional("id")] = parameters =>
            {
                Dinner dinner = DocumentSession.Load<Dinner>((int)parameters.id);

                if (!dinner.IsUserRegistered(this.Context.CurrentUser.UserName))
                {

                    RSVP rsvp = new RSVP();
                    rsvp.AttendeeNameId = this.Context.CurrentUser.UserName;
                    rsvp.AttendeeName = ((UserIdentity)this.Context.CurrentUser).FriendlyName;

                    dinner.RSVPs.Add(rsvp);

                    //DocumentSession.Store(rsvp);
                    DocumentSession.SaveChanges(); 
                }

                return "Thanks - we'll see you there!";
            };
        }
    }
}