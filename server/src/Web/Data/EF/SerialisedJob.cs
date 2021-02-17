namespace Web.Data.EF
{
    public record SerialisedJob
    {
        public long Id { get; set; }
        public int Retries { get; set; }
        public int Attempts { get; set; }
        public string Type { get; set; }
        public string Json { get; set; }
    }
}
