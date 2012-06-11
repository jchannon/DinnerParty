using Nancy;
using System.IO;
using DinnerParty.Properties;
using Nancy.Authentication.Forms;
using Nancy.Validation.DataAnnotations;
using DinnerParty.Models.CustomAnnotations;
using Nancy.Bootstrapper;
using DinnerParty.Models.RavenDB;
using Nancy.Diagnostics;
using System;
using System.Collections.Generic;
using Raven.Client;
using TinyIoC;

namespace DinnerParty
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private byte[] favicon;

        protected override byte[] DefaultFavIcon
        {
            get
            {
                if (favicon == null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Resources.favicon.Save(ms);
                        favicon = ms.ToArray();
                    }
                }
                return favicon;
            }
        }

        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            DataAnnotationsValidator.RegisterAdapter(typeof(MatchAttribute), (v, d) => new CustomDataAdapter((MatchAttribute)v));

            Func<TinyIoCContainer, NamedParameterOverloads, IDocumentSession> factory = (ioccontainer, namedparams) => { return new RavenSessionProvider().GetSession(); };
            container.Register<IDocumentSession>(factory);

           

           

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


    }
}