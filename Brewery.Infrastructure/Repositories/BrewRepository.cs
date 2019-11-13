using System;
using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Brewery.Infrastructure.Entities;

namespace Brewery.Infrastructure.Repositories
{
    public class BrewRepository : GenericRepository<BrewModel, Brew>, IBrewRepository
    {
        private readonly ApplicationDbContext context;

        public BrewRepository(ApplicationDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<BrewModel> GetBrewAndRelatedEntitiesAsync(Guid id)
        {
            context.Recipes.Load();
            context.Resources.Load();
            var result = await context.Brews.FirstOrDefaultAsync(x => x.BrewId.Equals(id));
            return EntityToModel(result);
        }

        public async Task<IEnumerable<BrewModel>> GetBrewsAndTheirRelatedEntitiesAsync(bool includeRemoved = false)
        {
            await context.Recipes.LoadAsync();
            await context.Resources.LoadAsync();
            await context.Produced.LoadAsync();

            List<Brew> brews = new List<Brew>();
            if (includeRemoved)
            {
                brews = await context.Brews.Include(x => x.Recipes).ToListAsync();
            }
            else
            {
                brews = await context.Brews.Where(x => !x.IsRemoved).Include(x => x.Recipes).ToListAsync();
            }

            List<BrewModel> models = new List<BrewModel>();
            foreach (var brew in brews)
            {
                var model = EntityToModel(brew);
                models.Add(model);
            }
            return models;
        }

        public override void Delete(Guid id)
        {
            Brew entity = context.Brews.FirstOrDefault(x => x.BrewId.Equals(id));
            entity.IsRemoved = true;
            context.SaveChanges();
        }

        public Task<bool> FindIfExistsAsync(Guid id)
        {
            return context.Brews.AnyAsync(x => x.BrewId == id);
        }

        public override async Task<int> CountAsync()
        {
            return await context.Brews.Where(x => !x.IsRemoved).CountAsync();
        }

        #region Protected Methods
        protected override BrewModel EntityToModel(Brew entity)
        {
            ICollection<Recipe> recipes = entity.Recipes;
            return new BrewModel()
            {
                BrewId = entity.BrewId,
                Description = entity.Description,
                IsRemoved = entity.IsRemoved,
                Link = entity.Link,
                Name = entity.Name,
                Produceds = entity.Produceds != null ? entity.Produceds.Select(x => new ProducedModel()
                {
                    Name = entity.Name,
                    ProducedId = x.ProducedId,
                    Amount = x.Amount,
                    CreateDate = x.CreateDate,
                    ExprireDate = x.ExprireDate
                }).ToList() : new List<ProducedModel>(),
                Recipes = recipes != null ? recipes.Select(x => new RecipeModel()
                {
                    Amount = x.Amount,
                    ResourceName = x.Resource.Name,
                    Description = x.Description,
                    ResourceId = x.ResourceId,
                    Resource = x.Resource != null ? new ResourceModel()
                    {
                        ResourceId = x.ResourceId,
                        Name = x.Resource.Name,
                        IsRemoved = x.Resource.IsRemoved,
                        AmountInStock = x.Resource.AmountInStock,
                        Link = x.Resource.Link,
                        Unit = x.Resource.Unit
                    } : new ResourceModel()
                }).ToList() : new List<RecipeModel>(),
            };
        }

        protected override Brew ModelToEntity(BrewModel model)
        {
            return new Brew()
            {
                BrewId = model.BrewId,
                Description = model.Description,
                IsRemoved = model.IsRemoved,
                Link = model.Link,
                Name = model.Name,
            };
        }
        #endregion

        public async Task<bool> FindIfNameIsUniqueAsync(string name)
        {
            return await context.Brews.AnyAsync(x => !x.IsRemoved && x.Name.Equals(name));
        }
    }
}
