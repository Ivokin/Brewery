using Brewery.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Brewery.Web.ViewModels
{
    public class ResourceViewModel
    {
        private string description;
        private string name;
        private decimal amount;
        private string unit;
        private string link;

        public ResourceViewModel()
        {
            ShowAlert = true;
        }

        public bool IsNew { get; set; }

        public int AmountInStock { get; set; }

        public Guid Id { get; set; }

        public bool? IsValid { get; set; }

        public bool ShowAlert { get; set; }

        [RegularExpression(ViewConstants.LinkRegex)]
        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
            }
        }

        [MaxLength(10)]
        [Required]
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
                OnChange();
            }
        }

        [MaxLength(50)]
        [Required]
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

        [MaxLength(100)]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnChange();
            }
        }

        [Required]
        public decimal Amount
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

        public void OnChange()
        {
            if (!string.IsNullOrEmpty(Name) && (string.IsNullOrEmpty(Description) || Description.Length < 100) && !string.IsNullOrEmpty(Link) && (!string.IsNullOrEmpty(Unit)))
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }
    }
}
