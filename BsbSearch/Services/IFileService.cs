using BsbSearch.Models;

namespace BsbSearch.Services
{
    public interface IFileService
    {
        Task<List<BsbRecord>?> GetAllBsbRecords();
        Task UpdateBsbRecord(List<BsbRecord> bsbRecords);
        Task<List<Partner>?> GetAllPartners();
        Task AddRequestHistory(List<RequestHistory> requestHistory);
        Task<List<RequestHistory>?> GetAllRequestHistories();
    }
}
