using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Brewery.Core.Interfaces.Repositories;
using Brewery.Core.Models;
using Brewery.Infrastructure.Entities;

namespace Brewery.Infrastructure.Repositories
{
    public class ProducedGoodsRepository : GenericRepository<ProducedModel, Produced>, IProducedGoodsRepository
    {
        private readonly ApplicationDbContext context;
        public ProducedGoodsRepository(ApplicationDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public IEnumerable<ProducedModel> GetAllProducedModels()
        {
            context.Brews.Load();
            var entities = context.Produced;
            return entities.Select(x => EntityToModel(x));
        }

        public async Task<bool> FindIfExistsAsync(Guid id)
        {
            return await context.Produced.AnyAsync(x => x.ProducedId == id);
        }

        #region Protected Methods
        protected override ProducedModel EntityToModel(Produced entity)
        {
            return new ProducedModel()
            {
                Amount = entity.Amount,
                Brew = entity.Brew != null ? new BrewModel()
                {
                    BrewId = entity.BrewId,
                    Description = entity.Brew.Description,
                    IsRemoved = entity.Brew.IsRemoved,
                    Link = entity.Brew.Link,
                    Name = entity.Brew.Name,
                } : null,
                BrewId = entity.BrewId,
                CreateDate = entity.CreateDate,
                ExprireDate = entity.ExprireDate,
                ProducedId = entity.ProducedId
            };
        }

        protected override Produced ModelToEntity(ProducedModel model)
        {
            return new Produced()
            {
                Amount = model.Amount,
                BrewId = model.BrewId,
                CreateDate = model.CreateDate,
                ExprireDate = model.ExprireDate,
                ProducedId = model.ProducedId
            };
        }
        #endregion
    }
}
