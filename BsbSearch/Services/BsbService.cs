using BsbSearch.Models;
using BsbSearch.ViewModels;
using System.Text;
using System.Text.Json;

namespace BsbSearch.Services
{
    public class BsbService : IBsbService
    {
        private readonly ILogger<BsbService> _logger;
        private readonly IFileService _fileService;
        private readonly IPartnerService _partnerService;
        private IHttpClientFactory _clientFactory;

        public BsbService(
            ILogger<BsbService> logger, 
            IFileService fileService, 
            IPartnerService partnerService,
            IHttpClientFactory clientFactory) =>
            (_logger, _fileService, _partnerService, _clientFactory) = 
            (logger, fileService, partnerService, clientFactory);

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

            // Notify all partners about the change
            var allPartners = await _partnerService.GetAllPartners();

            if (allPartners == null)
            {
                _logger.LogInformation("No partner found to be notified about changing BSB {bsb}", bsbRecord.Number);
                return;
            }

            var clientlocal = _clientFactory.CreateClient("LocalBackendApi");
            foreach (var partner in allPartners)
            {
                try
                {
                    _logger.LogInformation("Notifying partner {name} about changing BSB {bsb}", partner.Name, bsbRecord.Number);
                    var stringPayload = JsonSerializer.Serialize(new IncomingRequest { Url = $"api/bsb/{bsbRecord.Number}" });
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await clientlocal.PostAsync(partner.Url, httpContent);
                    result.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to Notify partner {name} about changing BSB {bsb}", partner.Name, bsbRecord.Number);
                    throw;
                }

            }
        }
    }
}
