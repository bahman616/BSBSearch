using BsbSearch.Models;

namespace BsbSearch.Services
{
    public interface IFileService
    {
        List<BsbRecord>? GetAllBsbRecords();
    }
}
