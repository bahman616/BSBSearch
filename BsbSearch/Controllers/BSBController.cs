using BsbSearch.Models;
using BsbSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace BsbSearch.Controllers
{
    [Route("api/[controller]")]
    public class BsbController : Controller
    {
        private ILogger<BsbController> _logger;
        private IBsbService _bsbService;
        public BsbController(ILogger<BsbController> logger, IBsbService bsbService) =>
            (_logger, _bsbService) = (logger, bsbService);

        [HttpGet]
        public List<BsbRecord>? Get()
        {
            _logger.LogInformation("Getting all Bsbs");
            return _bsbService.GetAllBsbRecords();
        }

        [HttpGet("{bsb}")]
        public BsbRecord? Get(string bsb)
        {
            _logger.LogInformation("Getting Bsb: {bsb}", bsb);
            return _bsbService.GetBsbRecord(bsb);
        }
    }
}
