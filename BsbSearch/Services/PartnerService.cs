namespace BsbSearch.Services
{
    public class PartnerService: IPartnerService
    {
        private readonly ILogger<PartnerService> _logger;
        private readonly IFileService _fileService;

        public PartnerService(ILogger<PartnerService> logger, IFileService fileService) =>
            (_logger, _fileService) = (logger, fileService);

        public async Task<bool> IsKeyValid(string name, string key) 
        {
            _logger.LogInformation("Validating team {name} to access resources", name);
            var partners = await this._fileService.GetAllPartners();
            if (partners == null)
            {
                _logger.LogError("No Partner Found!");
                return false;
            }

            return partners.Any(p => p.Name == name && p.Key == key);
        }
    }
}
