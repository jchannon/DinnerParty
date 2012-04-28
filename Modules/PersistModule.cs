using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;

namespace DinnerParty.Modules
{
    public class PersistModule : BaseModule
    {
        public IDocumentSession DocumentSession
        {
            get { return Context.Items["RavenSession"] as IDocumentSession; }
        }

        public PersistModule()
        {
        }

        public PersistModule(string modulepath)
            : base(modulepath)
        {
        }
    }
}