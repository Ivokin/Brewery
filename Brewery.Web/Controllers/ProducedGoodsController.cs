using Brewery.Core.Interfaces.Services;
using Brewery.Core.Models;
using Brewery.Web.Service;
using Brewery.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Web.Controllers
{
    public class ProducedGoodsController : Controller
    {
        private readonly IProducedGoodsService producedGoodsService;
        private readonly IBrewService brewService;
        private readonly IResourceService resourceService;
        private readonly IRecipeService recipeService;
        private readonly IMapper mapper;

        public ProducedGoodsController(IProducedGoodsService producedGoodsService,
            IBrewService brewService,
            IResourceService resourceService,
            IRecipeService recipeService,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.producedGoodsService = producedGoodsService;
            this.brewService = brewService;
            this.resourceService = resourceService;
            this.recipeService = recipeService;
        }

        public IActionResult Index()
        {
            List<ProducedModel> producedModels = producedGoodsService.GetProducedGoods();
            List<ProducedGoodsViewModel> producedGoodsViewModel = mapper.ProducedModelToProducedGoodsViewModel(producedModels);
            return View(producedGoodsViewModel);
        }

        public async Task<IActionResult> Create(Guid id, int amount)
        {
            bool success = false;
            if (Guid.Empty == id)
            {
          
                throw new ArgumentNullException();
            }

            BrewModel brew = await brewService.GetByIdAsync(id, true);
            if (brew == null)
            {
           
                throw new ArgumentNullException();
            }

            ResourceModel model = new ResourceModel();
            if (amount > 0)
            {
                foreach (var recipe in brew.Recipes)
                {
                    ResourceModel resource = await resourceService.GetByIdAsync(recipe.ResourceId);
                    resource.AmountInStock -= (amount * (int)recipe.Amount);

                    if (model != null)
                    {
                        model = resourceService.UpdateResourceAmount(resource.ResourceId, resource.AmountInStock);
                    }
                }

                if (model != null)
                {
                    DateTime today = DateTime.Today;
                    ProducedModel producedModel = new ProducedModel()
                    {
                        Amount = amount,
                        BrewId = brew.BrewId,
                        CreateDate = today.Date,
                        ExprireDate = today.AddMonths(6)
                    };

                    ProducedModel produced = await producedGoodsService.InsertAsync(producedModel);
                    success = produced != null;
                }
                else
                {
                    throw new ArgumentNullException(nameof(model));
                }
            }
            ProducedModel[] producedModels = producedGoodsService.GetProducedGoods().ToArray();

            return Json(new
            {
                success = success,
                producedGoods = producedModels
            });
        }
    }
}