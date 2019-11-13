using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Services
{
    public interface IBrewService
    {
        Task<List<BrewModel>> GetAllAsync();

        Task<List<BrewModel>> GetRandomFeaturedBrewsAsync(int number);

        Task AddBrewAndRecipesAsync(BrewModel newBrew);

        BrewModel Update(BrewModel brew);

        Task<bool> FindIfExistsAsync(Guid id);

        void Delete(Guid id);

        Task<BrewModel> GetByIdAsync(Guid id, bool getRelated);

        Task<bool> FindIfNameIsUniqueAsync(string name);
    }
}
