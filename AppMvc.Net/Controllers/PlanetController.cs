using AppMvc.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Net.Controllers
{
    [Route("he-mat-troi/[action]")]

    public class PlanetController : Controller
    {


        private readonly PlanetService _planetService;
        private readonly ILogger<PlanetController> _logger;
        public PlanetController(PlanetService planetService, ILogger<PlanetController> logger)
        {
            _planetService = planetService;
            _logger = logger;
        }
        // GET: Planet
        public ActionResult Index()
        {
            return View();
        }
        [BindProperty(SupportsGet = true, Name = "action")]
        public string Name { get; set; } // Action ~ PlanetModel


        public IActionResult Mercury() // he-mat-troi/Mercury
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            _logger.LogDebug("Name:", Name);
            return View("Detail", planet);
        }
        public IActionResult Venus()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Earth()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Mars()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        [HttpGet("/saomoc.html")]
        public IActionResult Jupiter()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Saturn()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Uranus()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
        [Route("sao/[action]", Order = 3, Name = "neptune3")]                 //  sao/Neptune
        [Route("sao/[controller]/[action]", Order = 2, Name = "neptune2")]    //  sao/Planet/Neptune
        [Route("[controller]-[action].html", Order = 1, Name = "neptune1")]   //  * Planet-Neptune.html
        public IActionResult Neptune()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        // controller, action, area => [controller] [action] [area]




        [Route("hanhtinh/{id:int}")] // hanhtinh/1
        public IActionResult PlanetInfo(int id)
        {

            var planet = _planetService.Where(p => p.Id == id).FirstOrDefault();
            return View("Detail", planet);
        }
    }
}
