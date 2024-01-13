using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
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
        [JsonProperty("id")]
        public string? DrugID { get; set; }
        [JsonProperty("drug")]
        [JsonPropertyName("drug")]
        [BsonElement("drug")]
        public string? Drug { get; set; }
        [JsonProperty("class")]
        [JsonPropertyName("class")]
        [BsonElement("class")]
        public string? DrugClass { get; set; }
        [JsonProperty("crcl")]
        [JsonPropertyName("crcl")]
        [BsonElement("crcl")]
        public string? Crcl { get; set; }
        [JsonProperty("disease")]
        [JsonPropertyName("disease")]
        [BsonElement("disease")]
        public string? Disease { get; set; }
        [JsonProperty("recommendation")]
        [JsonPropertyName("recommendtion")]
        [BsonElement("recommendation")]
        public string Recommendation { get; set; } = null!;
        [JsonProperty("rationale")]
        [JsonPropertyName("rationale")]
        [BsonElement("rationale")]
        public string Rationale { get; set; } = null!;
        [JsonProperty("qe")]
        [JsonPropertyName("qe")]
        [BsonElement("qe")]
        public string QualityEvidence { get; set; } = null!;
        [JsonProperty("rs")]
        [JsonPropertyName("rs")]
        [BsonElement("rs")]
        public string StrengthRecommendation { get; set; } = null!;
        [JsonProperty("condition")]
        [JsonPropertyName("condition")]
        [BsonElement("condition")]
        public string? Condition { get; set; }
        [JsonProperty("age")]
        [JsonPropertyName("age")]
        [BsonElement("age")]
        public string? Age { get; set; }
        [JsonProperty("interacting_drug_or_class")]
        [JsonPropertyName("interacting_drug_or_class")]
        [BsonElement("interacting_drug_or_class")]
        public string? InteractingDrugOrClass { get; set; }
        [JsonProperty("dosage")]
        [JsonPropertyName("dosage")]
        [BsonElement("dosage")]
        public string? Dosage { get; set; }


    }
}


