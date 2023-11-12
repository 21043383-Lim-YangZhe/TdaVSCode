using System.Text.Json.Serialization;

namespace TDA_WebApplication.Models
{
    public class Beers
    {
        public string DrugID { get; set; } = null!;

        public string Drug { get; set; } = null!;

        public string DrugClass { get; set; } = null!;

        public string? Crcl { get; set; }

        public string? Disease { get; set; }

        public string Recommendation { get; set; } = null!;

        public string Rationale { get; set; } = null!;

        public string QualityEvidence { get; set; } = null!;

        public string StrengthRecommendation { get; set; } = null!;

        public string? Condition { get; set; }

        public string? Age { get; set; }

        public string? InteractingDrugOrClass { get; set; }

        public string? Dosage { get; set; }
    }
}
