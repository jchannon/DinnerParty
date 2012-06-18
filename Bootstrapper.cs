using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Validation.DataAnnotations;
using DinnerParty.Models.CustomAnnotations;
using Nancy.Bootstrapper;
using DinnerParty.Models.RavenDB;
using Nancy.Diagnostics;
using System;
using Raven.Client;
using TinyIoC;
using DinnerParty.Models;
using Raven.Abstractions.Data;

namespace DinnerParty
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private byte[] favicon;

        //protected override byte[] DefaultFavIcon
        //{
        //    get
        //    {
        //        if (favicon == null)
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                Resource1.favicon.Save(ms);
        //                favicon = ms.ToArray();
        //            }
        //        }
        //        return favicon;
        //    }
        //}

        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            DataAnnotationsValidator.RegisterAdapter(typeof(MatchAttribute), (v, d) => new CustomDataAdapter((MatchAttribute)v));

            Func<TinyIoCContainer, NamedParameterOverloads, IDocumentSession> factory = (ioccontainer, namedparams) => { return new RavenSessionProvider().GetSession(); };
            container.Register<IDocumentSession>(factory);



            CleanUpDB(container.Resolve<IDocumentSession>());

            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(IndexEventDate).Assembly, RavenSessionProvider.DocumentStore);
            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(IndexUserLogin).Assembly, RavenSessionProvider.DocumentStore);
            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(IndexMostPopularDinners).Assembly, RavenSessionProvider.DocumentStore);
            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(IndexMyDinners).Assembly, RavenSessionProvider.DocumentStore);

            pipelines.OnError += (context, exception) =>
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                return null;
            };
        }

        protected override void RequestStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            // At request startup we modify the request pipelines to
            // include forms authentication - passing in our now request
            // scoped user name mapper.
            //
            // The pipelines passed in here are specific to this request,
            // so we can add/remove/update items in them as we please.
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/account/logon",
                    UserMapper = container.Resolve<IUserMapper>(),
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ConfigureRequestContainer(TinyIoC.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IUserMapper, UserMapper>();
        }

        protected override Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                var config = NancyInternalConfiguration.WithOverrides(x => x.NancyModuleBuilder = typeof(RavenAwareModuleBuilder));

                //config = config.ErrorHandlers = new List<Type>
                //                              {
                //                                  typeof (ErrorHandlers.Generic404ErrorHandler),
                //                                  typeof (Api.ErrorHandlers.Api404ErrorHandler),
                //                                  typeof (Nancy.ErrorHandling.DefaultErrorHandler),
                //                              };

                return config;
            }
        }

        protected override Nancy.Diagnostics.DiagnosticsConfiguration DiagnosticsConfiguration
        {

            get { return new DiagnosticsConfiguration { Password = @"nancy" }; }

        }

        private void CleanUpDB(IDocumentSession DocSession)
        {
            var configInfo = DocSession.Load<Config>("DinnerParty/Config");


            if (configInfo == null)
            {
                configInfo = new Config();
                configInfo.Id = "DinnerParty/Config";
                configInfo.LastTruncateDate = DateTime.Now.AddHours(-48); //No need to delete data if config doesnt exist but setup ready for next time

                DocSession.Store(configInfo);
                DocSession.SaveChanges();

                return;
            }
            else
            {
                if ((DateTime.Now - configInfo.LastTruncateDate).TotalHours < 24)
                    return;


                configInfo.LastTruncateDate = DateTime.Now;
                DocSession.SaveChanges();

                //If database size >15mb or 1000 documents delete documents older than a week

#if DEBUG
                var jsonData = RavenSessionProvider.DocumentStore.JsonRequestFactory.CreateHttpJsonRequest(null, "http://localhost:8080/database/size", "GET", RavenSessionProvider.DocumentStore.Credentials, RavenSessionProvider.DocumentStore.Conventions).ReadResponseJson();
#else
                var jsonData = RavenSessionProvider.DocumentStore.JsonRequestFactory.CreateHttpJsonRequest(null, "https://1.ravenhq.com/databases/DinnerParty-DinnerPartyDB/database/size", "GET", RavenSessionProvider.DocumentStore.Credentials, RavenSessionProvider.DocumentStore.Conventions).ReadResponseJson();
      
#endif
                int dbSize = int.Parse(jsonData.SelectToken("DatabaseSize").ToString());
                long docCount = RavenSessionProvider.DocumentStore.DatabaseCommands.GetStatistics().CountOfDocuments;

                
                if (docCount > 1000 || dbSize > 15000000) //its actually 14.3mb but goood enough
                {

                    RavenSessionProvider.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName",
                                              new IndexQuery
                                              {
                                                  Query = DocSession.Advanced.LuceneQuery<object>()
                                                  .WhereEquals("Tag", "Dinners")
                                                  .AndAlso()
                                                  .WhereLessThan("LastModified", DateTime.Now.AddDays(-7)).ToString()
                                              },
                                              false);
                }
            }

        }
    }


}