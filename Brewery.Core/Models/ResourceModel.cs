using System;

namespace Brewery.Core.Models
{
    public class ResourceModel
    {
        public Guid ResourceId { get; set; }

        public string Unit { get; set; }

        public string Link { get; set; }

        public int AmountInStock { get; set; }

        public string Name { get; set; }

        public bool IsRemoved { get; set; }
    }
}
