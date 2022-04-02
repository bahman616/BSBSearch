using Microsoft.AspNetCore.Mvc;

namespace BsbSearch.Controllers
{
    public class BsbController : Controller
    {
        [Route("api/[controller]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
