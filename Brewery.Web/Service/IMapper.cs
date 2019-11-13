using Brewery.Core.Models;
using Brewery.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace Brewery.Web.Service
{
    public interface IMapper
    {
        ResourceViewModel ResourceToResourceViewModel(ResourceModel resource);

        ResourceModel ResourceViewModelToResource(ResourceViewModel resource);

        BrewModel BrewsViewModelToBrew(BrewsViewModel brew);

        BrewsViewModel BrewToBrewsViewModel(BrewModel brew, bool includeRecipes);

        RecipeModel RecipeModelToRecipe(RecipeModel recipe, Guid brewId, Guid resourceId);

        List<ProducedGoodsViewModel> ProducedModelToProducedGoodsViewModel(List<ProducedModel> models);
    }
}
