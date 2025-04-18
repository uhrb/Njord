using Njord.Ais.Extensions.Types;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.Ais.SerDe.JSON
{
    public sealed class JsonIntToMMSIStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetInt32().ToMMSIFormattedString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(int.Parse(value));
        }
    }
}
