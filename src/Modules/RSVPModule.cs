using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using DinnerParty.Models;
using Nancy.RouteHelpers;
using Raven.Client;

namespace DinnerParty.Modules
{
    public class RSVPAuthorizedModule : BaseModule
    {
        public RSVPAuthorizedModule(IDocumentSession documentSession)
            : base("/RSVP")
        {
            this.RequiresAuthentication();

            Post["/Cancel/{id}"] = parameters =>
            {
                Dinner dinner = documentSession.Load<Dinner>((int)parameters.id);

                RSVP rsvp = dinner.RSVPs
                    .Where(r => this.Context.CurrentUser.UserName == (r.AttendeeNameId ?? r.AttendeeName))
                    .SingleOrDefault();

                if (rsvp != null)
                {
                    dinner.RSVPs.Remove(rsvp);
                    documentSession.SaveChanges();

                }

                return "Sorry you can't make it!";
            };

            Post["/Register/{id}"] = parameters =>
            {
                Dinner dinner = documentSession.Load<Dinner>((int)parameters.id);

                if (!dinner.IsUserRegistered(this.Context.CurrentUser.UserName))
                {

                    RSVP rsvp = new RSVP();
                    rsvp.AttendeeNameId = this.Context.CurrentUser.UserName;
                    rsvp.AttendeeName = ((UserIdentity)this.Context.CurrentUser).FriendlyName;

                    dinner.RSVPs.Add(rsvp);

                    documentSession.SaveChanges(); 
                }

                return "Thanks - we'll see you there!";
            };
        }
    }
}