using BsbSearch.Models;

namespace BsbSearch.Services
{
    public class BsbService : IBsbService
    {
        private readonly ILogger<BsbService> _logger;
        private readonly IFileService _fileService;

        public BsbService(ILogger<BsbService> logger, IFileService fileService) =>
            (_logger, _fileService) = (logger, fileService);

        public async Task<List<BsbRecord>?> GetAllBsbRecords() => 
            (await _fileService.GetAllBsbRecords())?
            .OrderBy(b => b.Number).ToList();

        public async Task<BsbRecord?> GetBsbRecord(string bsb)
        {
            if (string.IsNullOrEmpty(bsb))
            {
                _logger.LogError("Bsb number cannot be empty");
                throw new ArgumentNullException(nameof(bsb));
            }

            if (bsb.Length != 6)
            {
                _logger.LogError("Bsb number should be 6 characters");
                throw new ArgumentOutOfRangeException(nameof(bsb));
            }

            var allBsbRecords = await _fileService.GetAllBsbRecords();
            return allBsbRecords?.Where(bsbRecord => bsbRecord.Number == bsb).SingleOrDefault();
        }

        public async Task UpdateBsbRecord(string id, BsbRecord bsbRecord) {
            var allBsbRecords = await GetAllBsbRecords();

            if (allBsbRecords != null)
            {
                var itemsExcludingTheUpdatedOne = allBsbRecords.Where(b => b.Id != id).ToList();
                itemsExcludingTheUpdatedOne.Add(bsbRecord);
                await _fileService.UpdateBsbRecord(itemsExcludingTheUpdatedOne);
            }
            else 
            { 
                await _fileService.UpdateBsbRecord(new List<BsbRecord>() { bsbRecord });
            }
        }
    }
}
