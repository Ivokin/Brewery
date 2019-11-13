using Brewery.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Services
{
    public interface IProducedGoodsService
    {
        Task<ProducedModel> InsertAsync(ProducedModel produced);

        List<ProducedModel> GetProducedGoods();
    }
}
