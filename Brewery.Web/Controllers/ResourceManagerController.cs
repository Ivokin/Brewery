using Brewery.Web.Service;
using Brewery.Web.ViewModels;
using Brewery.Core.Constants;
using Brewery.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using Brewery.Core.Interfaces.Services;

namespace Brewery.Web.Controllers
{
    [Authorize(Roles = AccountConstants.AdminRole)]
    public class ResourceManagerController : Controller
    {
        private readonly IMapper mapper;
        private readonly IResourceService resourceService;

        public ResourceManagerController(IResourceService resourceService, IMapper mapper)
        {
            this.resourceService = resourceService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<ResourceViewModel> resources = new List<ResourceViewModel>();
            var resourceModels = await resourceService.GetAllAsync();
            resources = resourceModels.Where(x => !x.IsRemoved).Select(x => mapper.ResourceToResourceViewModel(x)).ToList();
            return View(resources);
        }

        public IActionResult Create()
        {
            ResourceViewModel resource = new ResourceViewModel();
            resource.IsNew = true;
            return View(resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResourceViewModel resource)
        {
            bool isValid = resource.IsValid.Value;
            if (isValid && ModelState.IsValid)
            {
                await resourceService.InsertAsync(mapper.ResourceViewModelToResource(resource));
                return RedirectToAction(nameof(Index));
            }
            else
            {
                resource.IsValid = false;
                return View(resource);
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (Guid.Empty == id)
            {
                return NotFound();
            }

            var resource = await resourceService.GetByIdAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(mapper.ResourceToResourceViewModel(resource));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ResourceViewModel resource)
        {
            resource.IsValid = ModelState.IsValid || resource == null || string.IsNullOrEmpty(resource.Name) || Guid.Empty == resource.Id;
            bool isNameUnique = await resourceService.FindIfNameIsUniqueAsync(resource.Id, resource.Name);
            if (!resource.IsValid.Value && !isNameUnique)
            {
                return View(resource);
            }

            ResourceModel editedResource = new ResourceModel();
            editedResource = resourceService.Update(mapper.ResourceViewModelToResource(resource));
            if (!ResourceExists(resource.Id))
            {
                return NotFound();
            }

            if (editedResource == null)
            {
                return NotFound();
            }

            ResourceViewModel newResourceViewModel = mapper.ResourceToResourceViewModel(editedResource);
            newResourceViewModel.ShowAlert = true;
            return View(newResourceViewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (Guid.Empty == id)
            {
                NotFound();
            }

            bool result = await resourceService.DeleteAsync(id);

            return Json(new
            {
                success = result,
            });
        }

        #region Private Methods
        private bool ResourceExists(Guid id)
        {
            return resourceService.GetByIdAsync(id) != null;
        }
        #endregion
    }
}

