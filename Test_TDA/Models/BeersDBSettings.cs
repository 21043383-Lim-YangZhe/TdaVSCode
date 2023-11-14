namespace TDA.Models
{
    public class BeersDBSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BeersCollectionName { get; set; } = null!;
    }
}
