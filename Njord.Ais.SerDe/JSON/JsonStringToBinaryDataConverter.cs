using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.Ais.SerDe.JSON
{
    public sealed partial class JsonStringToBinaryDataConverter : JsonConverter<ReadOnlyMemory<byte>>
    {
        public override ReadOnlyMemory<byte> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var unicoded = reader.GetString();
            if (unicoded == null)
            {
                return new ReadOnlyMemory<byte>([]);
            }
            return new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(unicoded));
        }

        public override void Write(Utf8JsonWriter writer, ReadOnlyMemory<byte> value, JsonSerializerOptions options)
        {
            var sb = new StringBuilder();
            foreach (byte c in value.Span)
            {
                if (c > 127) // Non-ASCII characters
                {
                    sb.AppendFormat("\\u{0:X4}", c);
                }
                else
                {
                    sb.Append(c);
                }
            }

            writer.WriteStringValue(sb.ToString());
        }
    }
}
