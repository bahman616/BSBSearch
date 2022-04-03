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
        private IRequestHistoryService _requestHistory;
        public BsbController(ILogger<BsbController> logger, IBsbService bsbService, IRequestHistoryService requestHistory) =>
            (_logger, _bsbService, _requestHistory) = (logger, bsbService, requestHistory);

        [HttpGet]
        public async Task<List<BsbRecord>?> Get()
        {
            var requestHistory = new RequestHistory();
            requestHistory.Url = $"/api/bsb";
            requestHistory.TeamName = Request.Headers["team-name"];

            _logger.LogInformation("Getting all Bsbs");

            try
            {
                var results = await _bsbService.GetAllBsbRecords();
                await AddRequestHistory(requestHistory);
                return results;

            }
            catch (Exception ex) 
            {

                _logger.LogError(ex, "Failed to retreive BSB Records");

                requestHistory.Status = RequestStatus.Fail;
                requestHistory.StatusMessage = ex.Message;

                await AddRequestHistory(requestHistory);
                throw;
            }

        }

        [HttpGet("{bsb}")]
        public async Task<BsbRecord?> Get(string bsb)
        {
            var requestHistory = new RequestHistory();
            requestHistory.Url = $"/api/bsb/{bsb}";
            requestHistory.TeamName = Request.Headers["team-name"];

            _logger.LogInformation("Getting Bsb: {bsb}", bsb);
            try
            {
                var result = await _bsbService.GetBsbRecord(bsb);
                await AddRequestHistory(requestHistory);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retreive BSB Record {bsb}", bsb);
                
                requestHistory.Status = RequestStatus.Fail;
                requestHistory.StatusMessage = ex.Message;

                await AddRequestHistory(requestHistory);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]BsbRecord bsbRecord)
        {
            _logger.LogInformation("Updating bsb: {bsb}", bsbRecord.Number);
            await _bsbService.UpdateBsbRecord(id, bsbRecord);
        }

        private async Task AddRequestHistory(RequestHistory requestHistory)
        {
            // don't need to keep the logs for local calls from the UI
            if (requestHistory.TeamName != "local")
            {
                await _requestHistory.Add(requestHistory);
            }
        }
    }
}
