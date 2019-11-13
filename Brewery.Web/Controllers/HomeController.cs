using Microsoft.AspNetCore.Mvc;
using Brewery.Core.Constants;
using Brewery.Web.Service;
using System.Linq;
using System.Collections.Generic;
using Brewery.Web.ViewModels;
using Brewery.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Brewery.Web.Controllers
{
    public class HomeController : Controller
    {
        private IMapper mapper;
        private IBrewService brewService;

        public HomeController(IBrewService brewService, IMapper mapper)
        {
            this.brewService = brewService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var featuredBrews = await brewService.GetRandomFeaturedBrewsAsync(NumericConstants.AmountOfFeaturedBrewsInHome);
            return View(featuredBrews.Select(x => mapper.BrewToBrewsViewModel(x, true)).ToList());
        }

        [Route(RoutingConstants.Featured)]
        public IActionResult Featured(List<BrewsViewModel> featuredBrews)
        {
            return View(featuredBrews);
        }

        [Route(RoutingConstants.About)]
        public IActionResult About()
        {
            return View();
        }
    }
}
