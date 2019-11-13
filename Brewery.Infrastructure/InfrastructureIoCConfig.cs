using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Interfaces.Services;
using Brewery.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace Brewery.Infrastructure
{
    public class InfrastructureIoCConfig : IoCConfig
    {
        private Container container;
        private string connectionString;

        public InfrastructureIoCConfig(Container container, string connectionString, IServiceCollection services)
        {
            this.container = container;
            this.connectionString = connectionString;
        }

        public void RegisterDependencies()
        {
            container.Register<IBrewRepository, BrewRepository>(Lifestyle.Scoped);
            container.Register<IResourceRepository, ResourceRepository>(Lifestyle.Scoped);
            container.Register<IRecipeRepository, RecipeRepository>(Lifestyle.Scoped);
            container.Register<IProducedGoodsRepository, ProducedGoodsRepository>(Lifestyle.Scoped);
        }
    }
}
