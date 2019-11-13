using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brewery.Infrastructure;

namespace Brewery.Core.Interfaces.Repositories
{
    public interface IBrewRepository : IGenericRepository<BrewModel>
    {
        Task<bool> FindIfExistsAsync(Guid id);

        Task<BrewModel> GetBrewAndRelatedEntitiesAsync(Guid id);

        Task<IEnumerable<BrewModel>> GetBrewsAndTheirRelatedEntitiesAsync(bool includeRemoved = false);

        Task<bool> FindIfNameIsUniqueAsync(string name);

        Task<List<BrewModel>> GetAllAsync();
    }
}
