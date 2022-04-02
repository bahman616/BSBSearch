using BsbSearch.Models;

namespace BsbSearch.Services
{
    public class RequestHistoryService : IRequestHistoryService
    {
        private readonly ILogger<IRequestHistoryService> _logger;
        private readonly IFileService _fileService;

        public RequestHistoryService(ILogger<IRequestHistoryService> logger, IFileService fileService) =>
            (_logger, _fileService) = (logger, fileService);

        public async Task Add(RequestHistory requestHistory)
        {
            var requestHistories = await _fileService.GetAllRequestHistories();
            if (requestHistories == null)
            {
                requestHistories = new List<RequestHistory>();
            }
            requestHistories.Add(requestHistory);

            await this._fileService.AddRequestHistory(requestHistories);
        }

        public async Task<List<RequestHistory>?> GetAllRequestHistories() => 
            (await _fileService.GetAllRequestHistories())?.OrderByDescending(r => r.DateTimeInUTC).ToList();
    }
}
