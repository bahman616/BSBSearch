using BsbSearch.Models;

namespace BsbSearch.Services
{
    public interface IPartnerService
    {
        Task<bool> IsKeyValid(string name, string key);
        Task<List<Partner>?> GetAllPartners();
    }
}
