namespace Riktig.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Controllers;
    using RapidTransit.Integration;
    using RapidTransit.Integration.Web;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        IContainer _container;

        protected void Application_Start()
        {
            ConfigureContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<WebConfigurationProviderModule>();
            builder.RegisterModule<RabbitMqConfigurationModule>();
            builder.RegisterModule<WebServiceBusModule>();

            builder.RegisterType<ImageServiceConfigurationSettings>()
                   .As<ImageServiceSettings>()
                   .SingleInstance();


            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            _container = builder.Build();
        }

        protected void Application_End()
        {
            _container.Dispose();
        }
    }
}