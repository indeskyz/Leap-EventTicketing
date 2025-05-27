namespace EventTicketing.Infrastructure
{
    public class AppSettings
    {
        public required string Redis { get; init; }
        public required string DefaultConnection { get; init; }
    }
}
