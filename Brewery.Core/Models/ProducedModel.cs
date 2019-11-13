using System;
using System.Collections.Generic;

namespace Brewery.Core.Models
{
    public class ProducedModel
    {
        public string Name { get; set; }

        public Guid ProducedId { get; set; }

        public int Avaible { get; set; }

        public int Amount { get; set; }

        public DateTime ExprireDate { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid BrewId { get; set; }

        public BrewModel Brew { get; set; }

        public List<string> ResourceNames { get; set; }
    }
}
