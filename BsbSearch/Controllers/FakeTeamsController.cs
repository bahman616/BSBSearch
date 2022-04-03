using BsbSearch.Infrastructure;
using BsbSearch.Models;
using BsbSearch.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BsbSearch.Controllers
{
    [Route("api/[controller]")]
    public class FakeTeamsController : Controller
    {
        private ILogger<FakeTeamsController> _logger;
        private IHttpClientFactory _clientFactory;
        private readonly Configuration _configurations;
        public FakeTeamsController(
            ILogger<FakeTeamsController> logger, 
            IHttpClientFactory clientFactory,
            IOptions<Configuration> optionsConfiguration) =>
            (_logger, _clientFactory, _configurations) = (logger, clientFactory, optionsConfiguration.Value);

        [HttpPost("team1")]
        public async Task Team1([FromBody] IncomingRequest req)
        {
            _logger.LogInformation("Team1 received a request with url: {url}", req.Url);

            try
            {
                var clientlocal = _clientFactory.CreateClient("LocalBackendApi");
                clientlocal.DefaultRequestHeaders.Add(Configuration.TeamNameHeader, _configurations.Team1Name);
                clientlocal.DefaultRequestHeaders.Add(Configuration.TeamKeyHeader, _configurations.Team1Key);
                var response = await clientlocal.GetAsync(req.Url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var bsbRecord = JsonSerializer.Deserialize<BsbRecord?>(content, options);

                _logger.LogInformation("Team1 successfully received BSB changes : {bsb}", bsbRecord?.Number);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Team1 failed to call url: {url}", req.Url);
                throw;
            }
        }

        [HttpPost("team2")]
        public async Task Team2([FromBody] IncomingRequest req)
        {
            _logger.LogInformation("Team2 received a request with url: {url}", req.Url);

            try
            {
                var clientlocal = _clientFactory.CreateClient("LocalBackendApi");
                // These credentials can be moved to app settings
                clientlocal.DefaultRequestHeaders.Add(Configuration.TeamNameHeader, _configurations.Team2Name);
                clientlocal.DefaultRequestHeaders.Add(Configuration.TeamKeyHeader, _configurations.Team2Key);
                var response = await clientlocal.GetAsync(req.Url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var bsbRecord = JsonSerializer.Deserialize<BsbRecord?>(content, options);

                _logger.LogInformation("Team2 successfully received BSB changes : {bsb}", bsbRecord?.Number);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Team2 failed to call url: {url}", req.Url);
                throw;
            }
        }
    }
}
