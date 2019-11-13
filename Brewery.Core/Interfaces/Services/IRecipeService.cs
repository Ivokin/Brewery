using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Services
{
    public interface IRecipeService
    {
        IEnumerable<RecipeModel> GetRecipesWithResourcesByBrewId(Guid id);

        Task<bool> AddOrUpdateAsync(IEnumerable<RecipeModel> recipes, Guid brewId);
    }
}
