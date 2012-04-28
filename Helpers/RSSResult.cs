using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Syndication;
using System.Xml;
using DinnerParty.Models;
using Nancy;
using System.IO;

namespace DinnerParty.Modules
{
    public static class FormatterExtensions
    {
        public static Response AsRSS(this IResponseFormatter formatter, IEnumerable<Dinner> model, string RSSTitle)
        {
            return new RSSResponse(model, RSSTitle, formatter.Context.Request.Url);
        }
    }

    public class RSSResponse : Response
    {
        private string RSSTitle { get; set; }
        private Uri URL { get; set; }

        public RSSResponse(IEnumerable<Dinner> model, string RSSTitle, Uri URL)
        {
            this.RSSTitle = RSSTitle; ;
            this.URL = URL;

            this.Contents = GetXmlContents(model);
            this.ContentType = "application/rss+xml";
            this.StatusCode = HttpStatusCode.OK;
        }

        private Action<Stream> GetXmlContents(IEnumerable<Dinner> model)
        {
            var items = new List<SyndicationItem>();

            foreach (Dinner d in model)
            {
                string contentString = String.Format("{0} with {1} on {2:MMM dd, yyyy} at {3}. Where: {4}, {5}",
                            d.Description, d.HostedBy, d.EventDate, d.EventDate.ToShortTimeString(), d.Address, d.Country);

                var item = new SyndicationItem(
                    title: d.Title,
                    content: contentString,
                    itemAlternateLink: new Uri("http://nrddnr.com/" + d.DinnerID),
                    id: "http://nrddnr.com/" + d.DinnerID,
                    lastUpdatedTime: d.EventDate.ToUniversalTime()
                    );
                item.PublishDate = d.EventDate.ToUniversalTime();
                item.Summary = new TextSyndicationContent(contentString, TextSyndicationContentKind.Plaintext);
                items.Add(item);
            }

            SyndicationFeed feed = new SyndicationFeed(
                this.RSSTitle,
                this.RSSTitle, /* Using Title also as Description */
                this.URL,
                items);

            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);

            return stream =>
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    formatter.WriteTo(writer);

                }
            };
        }
    }
}
