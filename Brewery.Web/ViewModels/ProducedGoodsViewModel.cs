using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Brewery.Web.ViewModels
{
    public class ProducedGoodsViewModel
    {
        private int amount;
        private string name;

        public bool IsValid { get; set; }

        public DateTime ExpDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int Avaible { get; set; }

        public Guid Id { get; set; }

        public List<string> ResourceNames { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnChange();
            }
        }

        [Required]
        [Range(1, int.MaxValue)]
        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                OnChange();
            }
        }

        private void OnChange()
        {
            IsValid = Amount > 0 && !string.IsNullOrEmpty(Name);
        }
    }
}
