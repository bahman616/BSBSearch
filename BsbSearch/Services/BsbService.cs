using BsbSearch.Models;

namespace BsbSearch.Services
{
    public class BsbService : IBsbService
    {
        private readonly ILogger _logger;
        private readonly IFileService _fileService;

        public BsbService(ILogger logger, IFileService fileService) =>
            (_logger, _fileService) = (logger, fileService);

        public List<BsbRecord>? GetAllBsbRecords() => _fileService.GetAllBsbRecords();

        public BsbRecord? GetBsbRecord(string bsb)
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


            return _fileService.GetAllBsbRecords()?.Where(bsbRecord => bsbRecord.Number == bsb).SingleOrDefault();
        }
    }
}
