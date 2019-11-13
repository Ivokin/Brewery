using System;
using System.Collections.Generic;
using System.Text;

namespace Brewery.Core.Models
{
    public class AspNetRole
    {
        public AspNetRole()
        {
        }

        public string Id { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
    }
}
