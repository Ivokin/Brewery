using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Repositories
{
    public interface IResourceRepository : IGenericRepository<ResourceModel>
    {
        Task<List<ResourceModel>> GetAllAsync(string[] excludeResources);

        Task<List<ResourceModel>> GetAllAsync();

        ResourceModel GetByName(string name);

        Task<bool> FindIfNameIsUniqueAsync(Guid excludeId, string name);

        Task<bool> DeleteAsync(Guid id);

        ResourceModel UpdateResourceAmount(Guid Id, int amount);
    }
}
