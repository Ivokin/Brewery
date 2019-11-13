using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Models;
using Brewery.Infrastructure.Entities;
using Brewery.Core.Constants;

namespace Brewery.Infrastructure.Repositories
{
    public class RecipeRepository : GenericRepository<RecipeModel, Recipe>, IRecipeRepository
    {
        private readonly ApplicationDbContext context;

        public RecipeRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<RecipeModel> GetByBrewId(Guid brewId)
        {
            context.Resources.Load();
            var result = context.Recipes.Where(x => x.BrewId.Equals(brewId)).Select(x => EntityToModel(x));
            return result;
        }

        #region Protected Methods
        protected override RecipeModel EntityToModel(Recipe entity)
        {
            Brew entityBrew = entity.Brew;
            Entities.Resource entityResource = entity.Resource;
            return new RecipeModel()
            {
                Amount = entity.Amount,
                Brew = entity.Brew != null ? new BrewModel()
                {
                    BrewId = entityBrew.BrewId,
                    Description = entityBrew.Description != null ? entityBrew.Description : string.Empty,
                    IsRemoved = entityBrew.IsRemoved,
                    Link = entityBrew.Link != null ? entityBrew.Link : UnitConstants.NotAny,
                    Name = entityBrew.Name,
                } : null,
                BrewId = entity.BrewId,
                Description = entity.Description,
                Resource = new ResourceModel()
                {
                    AmountInStock = entityResource.AmountInStock,
                    IsRemoved = entityResource.IsRemoved,
                    Link = entityResource.Link != null ? entityResource.Link : UnitConstants.NotAny,
                    Name = entityResource.Name,
                    ResourceId = entityResource.ResourceId,
                    Unit = entityResource.Unit
                },
                ResourceId = entity.ResourceId
            };
        }

        protected override Recipe ModelToEntity(RecipeModel model)
        {
            Guid resourceId = context.Resources.FirstOrDefault(x => x.Name == model.Name).ResourceId;
            return new Recipe()
            {
                Amount = model.Amount,
                BrewId = model.BrewId,
                Description = model.Description,
                ResourceId = resourceId
            };
        }
        #endregion
    }
}
