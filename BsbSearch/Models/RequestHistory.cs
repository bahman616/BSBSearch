namespace BsbSearch.Models
{
    public record RequestHistory
    {
        public string TeamName { get; set; } = default!;
        public string Url { get; set; } = default!;
        public DateTime DateTimeInUTC { get; set; } = DateTime.UtcNow;
        public RequestStatus Status { get; set; } = RequestStatus.Success;
        public string StatusMessage { get; set; } = String.Empty;
    };

    public enum RequestStatus 
    { 
        Success = 0,
        Fail = -1
    }
}
