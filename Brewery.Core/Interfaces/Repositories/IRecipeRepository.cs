using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brewery.Core.Models;

namespace Brewery.Core.Interfaces.Repositories
{
    public interface IRecipeRepository : IGenericRepository<RecipeModel>
    {
        IEnumerable<RecipeModel> GetByBrewId(Guid brewId);
    }
}
