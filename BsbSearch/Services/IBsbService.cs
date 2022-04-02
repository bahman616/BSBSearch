using BsbSearch.Models;

namespace BsbSearch.Services
{
    public interface IBsbService
    {
        public List<BsbRecord>? GetAllBsbRecords();
        public BsbRecord? GetBsbRecord(string bsb);
    }
}
