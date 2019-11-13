using System;
using System.Collections.Generic;

namespace Brewery.Core.Models
{
    public class BrewModel
    {
        public Guid BrewId { get; set; }

        public string Link { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRemoved { get; set; }

        public ICollection<ProducedModel> Produceds { get; set; }

        public ICollection<RecipeModel> Recipes { get; set; }
    }
}