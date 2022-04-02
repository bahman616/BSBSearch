namespace BsbSearch.Services
{
    public interface IPartnerService
    {
        Task<bool> IsKeyValid(string name, string key);
    }
}
