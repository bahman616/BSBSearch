using BsbSearch.Models;

namespace BsbSearch.Services
{
    public interface IBsbService
    {
        public Task<List<BsbRecord>?> GetAllBsbRecords();
        public Task<BsbRecord?> GetBsbRecord(string bsb);
        public Task UpdateBsbRecord(string id, BsbRecord bsbRecord);
    }
}
