using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Interfaces.Services;
using Brewery.Core.Models;

namespace Brewery.BusinessLogic.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        public async Task<IEnumerable<ResourceModel>> GetAllAsync()
        {
            return await resourceRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ResourceModel>> GetAllAsync(string[] excludeResources)
        {
            List<ResourceModel> models = new List<ResourceModel>();
            if (excludeResources != null && excludeResources.Any())
            {
                models = await resourceRepository.GetAllAsync(excludeResources);
            }
            return models;
        }

        public async Task<bool> FindIfNameIsUniqueAsync(Guid id, string name)
        {
            if (Guid.Empty == id && string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await resourceRepository.FindIfNameIsUniqueAsync(id, name);
        }

        public ResourceModel Update(ResourceModel resource)
        {
            var result = resourceRepository.Update(resource);
            return result;
        }

        public async Task<ResourceModel> InsertAsync(ResourceModel resource)
        {
            ResourceModel model = resourceRepository.GetByName(resource.Name);
            if (model != null)
            {
                if (model.IsRemoved)
                {
                    model.Name = resource.Name;
                    model.IsRemoved = false;
                    model.AmountInStock = resource.AmountInStock;
                    resourceRepository.Update(model);
                }
            }
            else
            {
                model = await resourceRepository.AddAsync(resource);
            }

            return model;
        }

        public ResourceModel UpdateResourceAmount(Guid id, int amount)
        {
            return resourceRepository.UpdateResourceAmount(id,amount);
        }

        public async Task<ResourceModel> GetByIdAsync(Guid id)
        {
            ResourceModel resource = new ResourceModel();
            if (Guid.Empty != id)
            {
                resource = await resourceRepository.GetByIdAsync(id);
            }

            return resource;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool result = false;
            if (Guid.Empty != id)
            {
                result = await resourceRepository.DeleteAsync(id);
            }

            return result;
        }
    }
}
