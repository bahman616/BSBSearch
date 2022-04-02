using BsbSearch.Models;

namespace BsbSearch.Services
{
    public interface IRequestHistoryService
    {
        Task Add(RequestHistory requestHistory);
        Task<List<RequestHistory>?> GetAllRequestHistories();
    }
}
