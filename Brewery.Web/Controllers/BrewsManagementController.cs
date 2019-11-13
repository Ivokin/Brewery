using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brewery.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Brewery.Web.Service;
using Brewery.Web.ViewModels;
using System.Collections.Generic;
using Brewery.Core.Constants;
using System;
using Brewery.Core.Interfaces.Services;

namespace Brewery.Web.Controllers
{
    [Authorize(Roles = AccountConstants.AdminRole)]
    public class BrewsManagementController : Controller
    {
        private readonly IMapper mapper;
        private readonly IBrewService brewService;
        private readonly IResourceService resourceService;
        private readonly IRecipeService recipeService;

        public BrewsManagementController(IBrewService brewService, IResourceService resourceService, IRecipeService recipeService, IMapper mapper)
        {
            this.resourceService = resourceService;
            this.brewService = brewService;
            this.mapper = mapper;
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> Index()
        {
            List<BrewsViewModel> newBrewViewModel = new List<BrewsViewModel>();
            List<BrewModel> brews = await brewService.GetAllAsync();
            foreach (var brew in brews.Where(x => !x.IsRemoved))
            {
                newBrewViewModel.Add(mapper.BrewToBrewsViewModel(brew, true));
            }

            return View(newBrewViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var resources = await resourceService.GetAllAsync();
            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> resourcesSelect = resources.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = x.Unit, Text = x.Name }).ToList();
            var brew = new BrewsViewModel()
            {
                IsNew = true,
                ShowAlert = false,
                ResoucesSelect = resourcesSelect,
            };
            brew.Recipes.Add(new RecipeModel() { IsNew = true });
            return View(brew);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrewsViewModel brew)
        {
            if (brew == null)
            {
                NotFound();
            }

            string name = brew.Name;
            brew.IsNew = true;
            brew.IsValid = false;
            if (!ModelState.IsValid)
            {
                return View(brew);
            }

            if (!ValidateBrew(brew) || !await brewService.FindIfNameIsUniqueAsync(name))
            {
                return View(brew);
            }
            else
            {
                BrewModel newBrew = mapper.BrewsViewModelToBrew(brew);
                newBrew.Recipes = brew.Recipes;
                brewService.AddBrewAndRecipesAsync(newBrew);
                brew.IsValid = true;
                return RedirectToAction(RoutingConstants.BrewsManagementIndex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var brew = await GetMappedBrewViewModel(id);
            return View(brew);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrewsViewModel brew)
        {
            if (!ModelState.IsValid && !ValidateBrew(brew))
            {
                brew.IsValid = false;
                return View(brew);
            }
            else
            {
                BrewModel updatedBrew = brewService.Update(mapper.BrewsViewModelToBrew(brew));
                if (updatedBrew == null)
                {
                    NotFound();
                }
                else
                {
                    await recipeService.AddOrUpdateAsync(brew.Recipes, brew.BrewId);
                }

                brew.IsValid = true;
            }

            if (brew.IsValid.Value)
            {
                return RedirectToAction(RoutingConstants.BrewsManagementIndex);
            }
            else
            {
                return View(brew);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            if (Guid.Empty == id)
            {
                return NotFound();
            }

            bool result = brewService.FindIfExistsAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();
            if (result)
            {
                brewService.Delete(id);
            }

            return Json(new
            {
                success = result,
                data = id
            });
        }

        [HttpPost]
        public IActionResult GetOptions(Guid id, string[] resources)
        {
            List<ResourceModel> models = new List<ResourceModel>();
            if (resources != null && resources.Any())
            {
                models = resourceService.GetAllAsync(resources).Result.ToList();
            }

            return Json(new
            {
                success = true,
                data = models
            });
        }

        #region Private Methods
        private bool ValidateBrew(BrewsViewModel brew)
        {
            return brew != null && !string.IsNullOrEmpty(brew.Name)
                && brew.Name.Trim().Length < NumericConstants.BrewNameMaxLength
                && (!string.IsNullOrEmpty(brew.Description));
        }

        private async Task<BrewsViewModel> GetMappedBrewViewModel(Guid id)
        {
            var resources = await resourceService.GetAllAsync();
            BrewModel brewModel = await brewService.GetByIdAsync(id, true);
            BrewsViewModel brew = mapper.BrewToBrewsViewModel(brewModel, false);

            foreach (var recipe in brewModel.Recipes)
            {
                brew.Recipes.Add(new RecipeModel() { Amount = recipe.Amount, Unit = recipe.Resource.Unit, IsNew = false, ResourceName = recipe.Resource.Name, Description = recipe.Description });
            }

            brew.ResoucesSelect = resources.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = x.Unit, Text = x.Name }).ToList();

            if (!brew.Recipes.Any())
            {
                brew.Recipes.Add(new RecipeModel() { Unit = resources.FirstOrDefault().Unit, IsNew = true, ResourceName = resources.FirstOrDefault().Name });
            }

            return brew;
        }
        #endregion
    }
}
