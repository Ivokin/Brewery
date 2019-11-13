using System;

namespace Brewery.Core.Models
{
    public class RecipeModel
    {
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public Guid ResourceId { get; set; }

        public ResourceModel Resource { get; set; }

        public Guid BrewId { get; set; }

        public BrewModel Brew { get; set; }

        public string Name { get; set; }

        public string ResourceName { get; set; }

        public string ResourceDescription { get; set; }

        public string Unit { get; set; }

        public bool IsNew { get; set; }
    }
}
