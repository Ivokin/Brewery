using System;
using System.Collections.Generic;

namespace Brewery.Infrastructure.Entities
{
    public class Brew
    {
        public Brew()
        {
            Produceds = new HashSet<Produced>();
            Recipes = new HashSet<Recipe>();
        }

        public Guid BrewId { get; set; }

        public string Link { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRemoved { get; set; }

        public ICollection<Produced> Produceds { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}