using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Routing;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using Nancy.ViewEngines;
using Nancy.Validation;

namespace DinnerParty.Models.RavenDB
{

    public class RavenAwareModuleBuilder : INancyModuleBuilder
    {
        private readonly IViewFactory viewFactory;
        private readonly IResponseFormatterFactory responseFormatterFactory;
        private readonly IModelBinderLocator modelBinderLocator;
        private readonly IRavenSessionProvider _ravenSessionProvider;
        private readonly IModelValidatorLocator validatorLocator;

        public RavenAwareModuleBuilder(IViewFactory viewFactory, IResponseFormatterFactory responseFormatterFactory,
                                       IModelBinderLocator modelBinderLocator,
                                       IRavenSessionProvider ravenSessionProvider,
             IModelValidatorLocator validatorLocator)
        {
            this.viewFactory = viewFactory;
            this.responseFormatterFactory = responseFormatterFactory;
            this.modelBinderLocator = modelBinderLocator;
            this.validatorLocator = validatorLocator;
            _ravenSessionProvider = ravenSessionProvider;
        }

        public NancyModule BuildModule(NancyModule module, NancyContext context)
        {
            module.Context = context;
            module.Response = this.responseFormatterFactory.Create(context);
            module.ViewFactory = this.viewFactory;
            module.ModelBinderLocator = this.modelBinderLocator;
            module.ValidatorLocator = this.validatorLocator;

            if (module is DinnerParty.Modules.PersistModule)
            {
                context.Items.Add("RavenSession", _ravenSessionProvider.GetSession());
                //module.After.AddItemToStartOfPipeline(ctx =>
                //{
                //    var session =
                //        ctx.Items["RavenSession"] as IDocumentSession;
                //    session.SaveChanges();
                //    session.Dispose();
                //});
            }

            return module;
        }
    }

}