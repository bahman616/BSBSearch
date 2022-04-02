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
        public async Task<List<BsbRecord>?> Get()
        {
            _logger.LogInformation("Getting all Bsbs");
            var results = await _bsbService.GetAllBsbRecords();
            return results;
        }

        [HttpGet("{bsb}")]
        public async Task<BsbRecord?> Get(string bsb)
        {
            _logger.LogInformation("Getting Bsb: {bsb}", bsb);
            var result = await _bsbService.GetBsbRecord(bsb);
            return result;
        }

        [HttpPut("{id}")]
        public async Task Put(string id, BsbRecord bsbRecord)
        {
            _logger.LogInformation("Updating bsb: {bsb}", bsbRecord.Number);
            await _bsbService.UpdateBsbRecord(id, bsbRecord);
        }
    }
}
