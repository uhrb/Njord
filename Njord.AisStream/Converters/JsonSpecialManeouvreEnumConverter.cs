using Njord.Ais.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Converters
{
    public class JsonSpecialManeouvreEnumConverter : JsonConverter<SpecialManoeuvreIndicator>
    {
        public override SpecialManoeuvreIndicator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // bug in aisstream
            var val = reader.GetInt32();
            return val switch
            {
                var a when a == 0 => SpecialManoeuvreIndicator.NotAvailable,
                var a when a == 1 => SpecialManoeuvreIndicator.NotEngaged,
                var a when a == 2 => SpecialManoeuvreIndicator.Engaged,
                var a when a == 3 => SpecialManoeuvreIndicator.Engaged, // this fking bug
                var a => throw new JsonException($"Special Manoeuvre indicator has wrong value {a}")
            };
        }

        public override void Write(Utf8JsonWriter writer, SpecialManoeuvreIndicator value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((byte)value);
        }
    }
}
