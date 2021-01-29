namespace SintefSemantic.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Rfq
    {
        [JsonProperty("nda")]
        public string Nda { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("projectDescription")]
        public string ProjectDescription { get; set; }

        [JsonProperty("selectionType")]
        public string SelectionType { get; set; }

        [JsonProperty("supplierMaxDistance")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public long SupplierMaxDistance { get; set; }

        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("servicePolicy")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public bool ServicePolicy { get; set; }

        [JsonProperty("projectId")]
        public Guid ProjectId { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("projectType")]
        public string ProjectType { get; set; }

        [JsonProperty("projectAttributes")]
        public List<ProjectAttribute> ProjectAttributes { get; set; }

        [JsonProperty("supplierAttributes")]
        public List<SupplierAttribute> SupplierAttributes { get; set; }
    }

    public partial class Customer
    {
        [JsonProperty("customerInfo")]
        public CustomerInfo CustomerInfo { get; set; }
    }

    public partial class CustomerInfo
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }
    }

    public partial class ProjectAttribute
    {
        [JsonProperty("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [JsonProperty("attributeId")]
        public Guid AttributeId { get; set; }

        [JsonProperty("processName")]
        public string ProcessName { get; set; }

        [JsonProperty("attributeKey")]
        public string AttributeKey { get; set; }

        [JsonProperty("attributeValue")]
        public string AttributeValue { get; set; }
    }

    public partial class SupplierAttribute
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("attributeKey")]
        public string AttributeKey { get; set; }

        [JsonProperty("attributeValue")]
        public string AttributeValue { get; set; }
    }

    public partial class Rfq
    {
        public static Rfq FromJson(string json) => JsonConvert.DeserializeObject<Rfq>(json, SintefSemantic.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Rfq self) => JsonConvert.SerializeObject(self, SintefSemantic.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class PurpleParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (bool.TryParse(value, out var b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
        }

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (long.TryParse(value, out var l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }
}
