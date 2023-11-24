using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TdaWebApp.Models
{

    public class Beers
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [JsonPropertyName("id")]
        [BsonElement("id")]
        public string? DrugID { get; set; }
        [JsonPropertyName("drug")]
        [BsonElement("drug")]
        public string? Drug { get; set; }

        [JsonPropertyName("class")]
        [BsonElement("class")]
        public string? DrugClass { get; set; }

        [JsonPropertyName("crcl")]
        [BsonElement("crcl")]
        public string? Crcl { get; set; }
        [JsonPropertyName("disease")]
        [BsonElement("disease")]
        public string? Disease { get; set; }
        [JsonPropertyName("recommendtion")]
        [BsonElement("recommendation")]
        public string Recommendation { get; set; } = null!;
        [JsonPropertyName("rationale")]
        [BsonElement("rationale")]
        public string Rationale { get; set; } = null!;
        [JsonPropertyName("qe")]
        [BsonElement("qe")]
        public string QualityEvidence { get; set; } = null!;
        [JsonPropertyName("rs")]
        [BsonElement("rs")]
        public string StrengthRecommendation { get; set; } = null!;
        [JsonPropertyName("condition")]
        [BsonElement("condition")]
        public string? Condition { get; set; }
        [JsonPropertyName("age")]
        [BsonElement("age")]
        public string? Age { get; set; }
        [JsonPropertyName("interacting_drug_or_class")]
        [BsonElement("interacting_drug_or_class")]
        public string? InteractingDrugOrClass { get; set; }
        [JsonPropertyName("dosage")]
        [BsonElement("dosage")]
        public string? Dosage { get; set; }
    }
}
