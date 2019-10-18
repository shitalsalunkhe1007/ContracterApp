using ContactInformation.Services;
using ContactInformation.Contracts;
using ContactInformation.Data;
using ContactInformation.Repositories;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace ContactInformation.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IDataContext, DataContext>();
            container.RegisterType<IContactInformationService, ContactInformationService>();           
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}