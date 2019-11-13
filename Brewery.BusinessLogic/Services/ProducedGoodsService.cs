using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Interfaces.Services;
using Brewery.Core.Models;
using Brewery.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brewery.BusinessLogic.Services
{
    public class ProducedGoodsService : IProducedGoodsService
    {
        private readonly IProducedGoodsRepository producedGoodsRepository;
        private readonly IBrewRepository brewRepository;
        private readonly IRecipeRepository recipeRepository; 

        public ProducedGoodsService(IProducedGoodsRepository producedGoodsRepository, IBrewRepository brewRepository,IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
            this.brewRepository = brewRepository;
            this.producedGoodsRepository = producedGoodsRepository;
        }

        public async Task<ProducedModel> InsertAsync(ProducedModel produced)
        {
            ProducedModel newProduced = new ProducedModel();
            if (produced != null)
            {
                newProduced = await producedGoodsRepository.AddAsync(produced);
                producedGoodsRepository.SaveAsync();
            }

            return newProduced;
        }

        public List<ProducedModel> GetProducedGoods()
        {
            List<ProducedModel> produced = new List<ProducedModel>();

            var brews = brewRepository.GetBrewsAndTheirRelatedEntitiesAsync(false).Result;

            foreach (var brew in brews)
            {
                int avaible = brew.Recipes.Any() ? CountMaxProduction(brew.Recipes) : 0;

                produced.Add(new ProducedModel()
                {
                    BrewId = brew.BrewId,
                    Avaible = avaible,
                    Name = brew.Name,
                    ResourceNames = brew.Recipes.Select(x => x.ResourceName).ToList(),
                });
            }

            return produced;
        }

        #region Private Methods
        private int CountMaxProduction(IEnumerable<RecipeModel> recipes, int result = 0)
        {
            RecipeModel recipe = recipes.FirstOrDefault();

            result = recipe != null ? (recipe.Resource.AmountInStock / (int)recipe.Amount) : result;
            if (recipes.Any() && result >= 1)
            {
                result = CountMaxProduction(recipes.Skip(1), result);
            }

            return result;
        }
        #endregion
    }
}
