using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Services
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceModel>> GetAllAsync();

        Task<IEnumerable<ResourceModel>> GetAllAsync(string [] excludeResources);

        ResourceModel Update(ResourceModel model);

        Task<ResourceModel> InsertAsync(ResourceModel resource);

        Task<ResourceModel> GetByIdAsync(Guid id);

        Task<bool> FindIfNameIsUniqueAsync(Guid id, string name);

        Task<bool> DeleteAsync(Guid id);

        ResourceModel UpdateResourceAmount(Guid Id, int amount);
    }
}
