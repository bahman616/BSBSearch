using BsbSearch.Models;
using System.Text.Json;

namespace BsbSearch.Services
{
    public class FileService : IFileService
    {
        public async Task<List<BsbRecord>?> GetAllBsbRecords()
        {
            using (StreamReader r = new StreamReader("data/BsbDirectory.json"))
            {
                string json = await r.ReadToEndAsync();
                var items = JsonSerializer.Deserialize<List<BsbRecord>>(json);

                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }

                return items;
            }
        }

        public async Task UpdateBsbRecord(List<BsbRecord> bsbRecords)
        {
            using (StreamWriter w = new StreamWriter("data/BsbDirectory.json"))
            {
                var json = JsonSerializer.Serialize(bsbRecords);
                await w.WriteAsync(json);
            }
        }

        public async Task<List<Partner>?> GetAllPartners()
        {
            using (StreamReader r = new StreamReader("data/Partners.json"))
            {
                string json = await r.ReadToEndAsync();
                var items = JsonSerializer.Deserialize<List<Partner>>(json);

                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }

                return items;
            }
        }

        public async Task AddRequestHistory(List<RequestHistory> requestHistory)
        {
            using (StreamWriter w = new StreamWriter("data/RequestsHistory.json"))
            {
                var json = JsonSerializer.Serialize(requestHistory);

                await w.WriteAsync(json);
            }
        }

        public async Task<List<RequestHistory>?> GetAllRequestHistories()
        {
            using (StreamReader r = new StreamReader("data/RequestsHistory.json"))
            {
                string json = await r.ReadToEndAsync();
                
                if (string.IsNullOrEmpty(json))
                {
                    return null;
;                }

                var items = JsonSerializer.Deserialize<List<RequestHistory>>(json);
                return items;
            }
        }
    }
}
