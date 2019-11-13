using Brewery.BusinessLogic.Services;
using Brewery.Core.Interfaces.Services;
using Brewery.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace Brewery.BusinessLogic
{
    public class BusinessLogicIoCConfig : IoCConfig
    {
        private string connectionString;
        private readonly Container container;
        private readonly IServiceCollection services;

        public BusinessLogicIoCConfig(Container container, string connectionString, IServiceCollection services)
        {
            this.connectionString = connectionString;
            this.container = container;
            this.services = services;

            InfrastructureIoCConfig infrastructureIoCConfig = new InfrastructureIoCConfig(container, connectionString, services);
            infrastructureIoCConfig.RegisterDependencies();
        }

        public void RegisterDependencies()
        {
            container.Register<IResourceService, ResourceService>(Lifestyle.Scoped);
            container.Register<IBrewService, BrewService>(Lifestyle.Scoped);
            container.Register<IProducedGoodsService, ProducedGoodsService>(Lifestyle.Scoped);
            container.Register<IRecipeService, RecipeService>(Lifestyle.Scoped);
        }
    }
}
