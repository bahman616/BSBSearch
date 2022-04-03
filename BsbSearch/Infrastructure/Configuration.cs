namespace BsbSearch.Infrastructure
{
    public class Configuration
    {
        public const string TeamNameHeader = "team-name";
        public const string TeamKeyHeader = "very-very-secure";
        public string LocalTeamName { get; set; } = String.Empty;
        public string LocalTeamKey { get; set; } = String.Empty;
        public string Team1Name { get; set; } = String.Empty;
        public string Team1Key { get; set; } = String.Empty;
        public string Team2Name { get; set; } = String.Empty;
        public string Team2Key { get; set; } = String.Empty;
    }
}
