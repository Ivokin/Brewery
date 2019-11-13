using System;
using System.Collections.Generic;

namespace Brewery.Infrastructure.Entities
{
    public class Resource
    {
        public Resource()
        {
            Recipes = new HashSet<Recipe>();
        }

        public Guid ResourceId { get; set; }

        public string Unit { get; set; }

        public string Link { get; set; }

        public int AmountInStock { get; set; }

        public string Name { get; set; }

        public bool IsRemoved { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
