using Brewery.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Brewery.Core.Constants;

namespace Brewery.Web.ViewModels
{
    public class BrewsViewModel
    {
        private string link;
        private string name;
        private string description;

        public BrewsViewModel()
        {
            Recipes = new List<RecipeModel>();
            ShowAlert = true;
        }

        public bool? IsNew { get; set; }

        public List<RecipeModel> Recipes { get; set; }

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ResoucesSelect { get; set; }

        public bool? IsValid { get; set; }

        public bool ShowAlert { get; set; }

        public Guid BrewId { get; set; }

        [MaxLength(1000)]
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

        [Required]
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

        public void OnChange()
        {
            if (!string.IsNullOrEmpty(Name) &&
                Name.Trim().Length < 50)
            {
                if (!string.IsNullOrEmpty(Description) && Description.Length > 1000)
                {
                    IsValid = false;
                }
                else
                {
                    IsValid = true;
                }
            }
            else
            {
                IsValid = false;
            }
        }
    }
}
