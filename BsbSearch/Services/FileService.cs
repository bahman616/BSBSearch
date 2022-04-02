using BsbSearch.Models;
using System.Text.Json;

namespace BsbSearch.Services
{
    public class FileService : IFileService
    {
        public List<BsbRecord>? GetAllBsbRecords()
        {
            using (StreamReader r = new StreamReader("data/BsbDirectory.json"))
            {
                string json = r.ReadToEnd();
                var items = JsonSerializer.Deserialize<List<BsbRecord>>(json);
                return items;
            }
        }
    }
}
