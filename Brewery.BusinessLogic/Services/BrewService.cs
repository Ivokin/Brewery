using System;
using Brewery.Core.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brewery.Core.Helpers;
using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Interfaces.Services;
using Brewery.Core.Models;

namespace Brewery.BusinessLogic.Services
{
    public class BrewService : IBrewService
    {
        private readonly IBrewRepository brewRepository;
        private readonly IRecipeRepository recipeRepository;

        public BrewService(IBrewRepository brewRepository, IRecipeRepository recipeRepository)
        {
            this.brewRepository = brewRepository;
            this.recipeRepository = recipeRepository;
        }

        public async Task AddBrewAndRecipesAsync(BrewModel brew)
        {
            BrewModel newBrew = await brewRepository.AddAsync(brew);
            ICollection<RecipeModel> newRecipes = brew.Recipes.Select(x => new RecipeModel()
            {
               BrewId = newBrew.BrewId,
               Name = x.ResourceName,
               Description = x.Description,
               Amount = x.Amount
            }).ToList();
            recipeRepository.InsertRangeAsync(newRecipes);
        }

        public void Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                brewRepository.Delete(id);
            }
        }

        public async Task<bool> FindIfExistsAsync(Guid id)
        {
            var result = false;
            if (id != Guid.Empty)
            {
                result = await brewRepository.FindIfExistsAsync(id);
            }

            return result;
        }

        public async Task<List<BrewModel>> GetAllAsync()
        {
            return await brewRepository.GetAllAsync();
        }

        public async Task<BrewModel> GetByIdAsync(Guid id, bool getRelated)
        {
            BrewModel model = null;
            if (id != Guid.Empty)
            {
                model = getRelated ? await brewRepository.GetBrewAndRelatedEntitiesAsync(id) : await brewRepository.GetByIdAsync(id);
            }

            return model;
        }

        public async Task<bool> FindIfNameIsUniqueAsync(string name)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(name))
            {
                result = !await brewRepository.FindIfNameIsUniqueAsync(name);
            }

            return result;
        }

        public async Task<List<BrewModel>> GetRandomFeaturedBrewsAsync(int number)
        {
            Randomizer randomizer = new Randomizer();
            int countOfBrews = await brewRepository.CountAsync();
            List<BrewModel> featuredBrews = new List<BrewModel>();
            if (countOfBrews > 0)
            {
                var brews = await brewRepository.GetBrewsAndTheirRelatedEntitiesAsync(false);
                List<int> randomNumbers = randomizer.ReturnListOfUniqueRandomNumbers(countOfBrews, number);
                if (countOfBrews > Core.Constants.NumericConstants.MinimumNumberOfFeaturedBrews)
                {
                    foreach (var brew in randomNumbers)
                    {
                        featuredBrews.Add(brews.ElementAt(brew));
                    }
                }
                else
                {
                    featuredBrews = brews.Take(NumericConstants.AmountOfFeaturedBrewsInHome).ToList();
                }
            }

            return featuredBrews;
        }

        public BrewModel Update(BrewModel brew)
        {
            if (brew != null)
            {
                brew = brewRepository.Update(brew);
                brewRepository.SaveAsync();
            }

            return brew;
        }
    }
}
