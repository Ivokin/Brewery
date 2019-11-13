using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Interfaces.Services;
using Brewery.Core.Models;

namespace Brewery.BusinessLogic.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IResourceRepository resourceRepository;

        public RecipeService(IRecipeRepository recipeRepository, IResourceRepository resourceRepository)
        {
            this.recipeRepository = recipeRepository;
            this.resourceRepository = resourceRepository;
        }

        public async Task<bool> AddOrUpdateAsync(IEnumerable<RecipeModel> recipes, Guid brewId)
        {
            bool result = true;
            foreach (var recipe in recipes)
            {
                Guid id = resourceRepository.GetByName(recipe.ResourceName).ResourceId;
                if (id == Guid.Empty)
                {
                    result = false;
                }
                else
                {
                    if (recipe.IsNew)
                    {
                        await recipeRepository.AddAsync(new RecipeModel()
                        {
                            Amount = recipe.Amount,
                            Name = recipe.ResourceName,
                            BrewId = brewId,
                            ResourceId = id,
                            Description = recipe.Description
                        });
                    }
                    else
                    {
                        var entity = new RecipeModel
                        {
                            Amount = recipe.Amount,
                            Name = recipe.ResourceName,
                            BrewId = brewId,
                            ResourceId = id,
                            Description = recipe.Description,
                        };
                        recipeRepository.Update(entity);
                    }

                    recipeRepository.SaveAsync();
                }
            }

            return result;
        }

        public IEnumerable<RecipeModel> GetRecipesWithResourcesByBrewId(Guid id)
        {
            IEnumerable<RecipeModel> recipes = new List<RecipeModel>();
            if (id != Guid.Empty)
            {
                recipes = recipeRepository.GetByBrewId(id);
            }

            return recipes;
        }
    }
}
