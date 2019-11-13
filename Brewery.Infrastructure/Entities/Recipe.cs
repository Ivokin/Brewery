using System;

namespace Brewery.Infrastructure.Entities
{
    public class Recipe
    {
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public Guid ResourceId { get; set; }

        public virtual Resource Resource { get; set; }

        public Guid BrewId { get; set; }

        public virtual Brew Brew { get; set; }
    }
}
