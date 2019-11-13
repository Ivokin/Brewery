using Brewery.Core.Models;
using Brewery.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brewery.Web.Service
{
    public class Mapper : IMapper
    {
        public BrewModel BrewsViewModelToBrew(BrewsViewModel brew)
        {
            List<RecipeModel> recipes = brew.Recipes;
            return new BrewModel()
            {
                BrewId = brew.BrewId,
                Name = brew.Name,
                Description = brew.Description,
                Link = brew.Link,
                Recipes = recipes != null ? brew.Recipes.Select(x => new RecipeModel()
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    Name = x.ResourceName
                }).ToList() : new List<RecipeModel>()
            };
        }

        public BrewsViewModel BrewToBrewsViewModel(BrewModel brew, bool includeRecipes)
        {
            BrewsViewModel viewModel = new BrewsViewModel();
            viewModel.ShowAlert = false;
            viewModel.BrewId = brew.BrewId;
            viewModel.Name = brew.Name;
            viewModel.Description = brew.Description;
            viewModel.Link = brew.Link;

            if (includeRecipes && brew.Recipes.Any())
            {
                viewModel.Recipes = new List<RecipeModel>();
                viewModel.Recipes = brew.Recipes.Select(x => new RecipeModel()
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    Resource = x.Resource ?? new ResourceModel()
                }).ToList();
            }

            return viewModel;
        }

        public List<ProducedGoodsViewModel> ProducedModelToProducedGoodsViewModel(List<ProducedModel> producedModels)
        {
            return producedModels.Select(x => new ProducedGoodsViewModel()
            {
                Id = x.BrewId,
                Name = x.Name,
                Avaible = x.Avaible,
                Amount = x.Amount,
                ResourceNames = x.ResourceNames
            }).ToList();
        }

        public RecipeModel RecipeModelToRecipe(RecipeModel recipe, Guid brewId, Guid resourceId)
        {
            return new RecipeModel()
            {
                Amount = recipe.Amount,
                BrewId = brewId,
                ResourceId = resourceId,
                Description = recipe.Description,
            };
        }

        public ResourceViewModel ResourceToResourceViewModel(ResourceModel resource)
        {
            return new ResourceViewModel()
            {
                Link = resource.Link,
                Unit = resource.Unit.ToLower(),
                ShowAlert = false,
                Id = resource.ResourceId,
                Name = resource.Name,
                AmountInStock = resource.AmountInStock,
            };
        }

        public ResourceModel ResourceViewModelToResource(ResourceViewModel resource)
        {
            return new ResourceModel()
            {
                Unit = resource.Unit.ToLower(),
                Link = resource.Link,
                ResourceId = resource.Id,
                Name = resource.Name,
                AmountInStock = resource.AmountInStock
            };
        }
    }
}
