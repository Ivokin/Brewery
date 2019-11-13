using System;

namespace Brewery.Infrastructure.Entities
{
    public class Produced
    {
        public Guid ProducedId { get; set; }

        public int Amount { get; set; }

        public DateTime ExprireDate { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid BrewId { get; set; }

        public virtual Brew Brew { get; set; }
    }
}
