using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.Ais.SerDe.JSON
{
    public class JsonInterfaceFactoryConverter<T, K> : JsonConverter<T> where K : T
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<K>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, typeof(K), options);
        }
    }
}
