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
        private static DocumentStore _documentStore;

        public static DocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static DocumentStore CreateDocumentStore()
        {
            DocumentStore store = new DocumentStore
            {
                ConnectionStringName = "RavenDB1"
            };
            
            store.Initialize();
          
            return store;
        }

        public IDocumentSession GetSession()
        {
            var session = DocumentStore.OpenSession();
            return session;
        }

       
    }
}