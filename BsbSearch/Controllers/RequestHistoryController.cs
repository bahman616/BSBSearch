using BsbSearch.Models;
using BsbSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace BsbSearch.Controllers
{
    [Route("api/[controller]")]
    public class RequestHistoryController : Controller
    {
        private ILogger<RequestHistoryController> _logger;
        private IRequestHistoryService _requestHistory;

        public RequestHistoryController(ILogger<RequestHistoryController> logger, IRequestHistoryService requestHistory) =>
            (_logger, _requestHistory) = (logger, requestHistory);


        [HttpGet]
        public async Task<List<RequestHistory>?> Index()
        {
            _logger.LogInformation("Getting the request history");
            return await _requestHistory.GetAllRequestHistories();
        }
    }
}
