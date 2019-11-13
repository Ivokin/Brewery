using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Models;
using Brewery.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brewery.Infrastructure.Repositories
{
    public class ResourceRepository : GenericRepository<ResourceModel, Resource>, IResourceRepository
    {
        private readonly ApplicationDbContext context;

        public ResourceRepository(ApplicationDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<bool> FindIfNameIsUniqueAsync(Guid excludeId, string name)
        {
            var result = await context.Resources.AnyAsync(x => x.Name == name && !x.ResourceId.Equals(excludeId));
            return !result;
        }

        public ResourceModel GetByName(string name)
        {
            Resource result = context.Resources.FirstOrDefault(x => x.Name.Equals(name));
            return EntityToModel(result);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Resource entity = await context.Resources.FirstOrDefaultAsync(x => x.ResourceId.Equals(id));
            entity.IsRemoved = true;
            SaveAsync();
            return entity.IsRemoved;
        }

        public async Task<List<ResourceModel>> GetAllAsync(string[] excludeResources)
        {
            return await context.Resources.Where(x => !excludeResources.Contains(x.Name)).Select(x => EntityToModel(x)).ToListAsync();
        }

        public ResourceModel UpdateResourceAmount(Guid Id, int amount)
        {
            var resource = context.Resources.FirstOrDefault(x => x.ResourceId == Id);
            if (resource != null)
            {
                resource.AmountInStock = amount;
            }

            SaveAsync();
            return EntityToModel(resource);
        }

        #region Protected Methods
        protected override ResourceModel EntityToModel(Resource entity)
        {
            ResourceModel model = entity != null ?
            new ResourceModel()
            {
                AmountInStock = entity.AmountInStock,
                IsRemoved = entity.IsRemoved,
                Link = entity.Link,
                Name = entity.Name,
                ResourceId = entity.ResourceId,
                Unit = entity.Unit
            } : null;

            return model;
        }

        protected override Resource ModelToEntity(ResourceModel model)
        {
            return new Resource()
            {
                AmountInStock = model.AmountInStock,
                IsRemoved = model.IsRemoved,
                Link = model.Link,
                Name = model.Name,
                ResourceId = model.ResourceId,
                Unit = model.Unit
            };
        }
        #endregion
    }
}
