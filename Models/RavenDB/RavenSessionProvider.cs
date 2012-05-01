using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Document;

namespace DinnerParty.Models.RavenDB
{
    public interface IRavenSessionProvider
    {
        IDocumentSession GetSession();
    }

    public class RavenSessionProvider : IRavenSessionProvider
    {
        private static IDocumentStore _documentStore;

        public static IDocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static IDocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            }.Initialize();

            return store;
        }

        public IDocumentSession GetSession()
        {
            var session = DocumentStore.OpenSession();
            return session;
        }

       
    }
}