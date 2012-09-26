namespace DinnerParty.Models.RavenDB
{
    using Nancy.Extensions;
    using Nancy.Responses.Negotiation;
    using Nancy.Routing;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.ViewEngines;
    using Nancy.Validation;

    public class RavenAwareModuleBuilder : INancyModuleBuilder
    {
        private readonly IViewFactory viewFactory;
        private readonly IResponseFormatterFactory responseFormatterFactory;
        private readonly IModelBinderLocator modelBinderLocator;
        private readonly IRavenSessionProvider ravenSessionProvider;
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
            this.ravenSessionProvider = ravenSessionProvider;
        }

        public NancyModule BuildModule(NancyModule module, NancyContext context)
        {
            CreateNegotiationContext(module, context);

            module.Context = context;
            module.Response = this.responseFormatterFactory.Create(context);
            module.ViewFactory = this.viewFactory;
            module.ModelBinderLocator = this.modelBinderLocator;
            module.ValidatorLocator = this.validatorLocator;

            if (module is Modules.PersistModule)
            {
                context.Items.Add("RavenSession", this.ravenSessionProvider.GetSession());
            }

            return module;
        }

        private static void CreateNegotiationContext(NancyModule module, NancyContext context)
        {
            context.NegotiationContext = new NegotiationContext
            {
                ModuleName = module.GetModuleName(),
                ModulePath = module.ModulePath,
            };
        }
    }

}