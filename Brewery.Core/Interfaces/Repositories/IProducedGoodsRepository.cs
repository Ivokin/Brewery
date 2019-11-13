using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Repositories
{
    public interface IProducedGoodsRepository : IGenericRepository<ProducedModel>
    {
        IEnumerable<ProducedModel> GetAllProducedModels();

        Task<bool> FindIfExistsAsync(Guid id);
    }
}
