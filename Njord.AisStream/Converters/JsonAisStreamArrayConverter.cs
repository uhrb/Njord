using Njord.Ais.SerDe.JSON;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Converters
{
    public class JsonAisStreamArrayConverter<T, K> : JsonConverter<IEnumerable<T>> where K : T
    {
        public override IEnumerable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);
            return BuildArrayValues(element, options);
        }

        private IEnumerable<T> BuildArrayValues(JsonElement element, JsonSerializerOptions options)
        {
            var count = element.GetPropertyCount();
            if (!options.Converters.Any(_ => _.Type == typeof(T)))
            {
                options = new JsonSerializerOptions(options);
                options.Converters.Add(new JsonInterfaceFactoryConverter<T, K>());
            }

            for (var idx = 0; idx < count; idx++)
            {
                var entry = element.GetProperty($"{idx}");
                var (result, shouldCommit, shouldContinue) = Deserialize(idx, entry, options);

                if (result != null && shouldCommit)
                {
                    yield return result;
                }

                if (!shouldContinue)
                {
                    break;
                }
            }

            yield break;
        }


        public virtual (T?, bool, bool) Deserialize(int index, JsonElement entry, JsonSerializerOptions options)
        {
            return (JsonSerializer.Deserialize<T>(entry, options), true, true);
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
        {
            var idx = 0;
            writer.WriteStartObject();
            if (!options.Converters.Any(_ => _.Type == typeof(T)))
            {
                options = new JsonSerializerOptions(options);
                options.Converters.Add(new JsonInterfaceFactoryConverter<T, K>());
            }
            foreach (var item in value)
            {
                writer.WritePropertyName($"{idx}");
                if (item == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    JsonSerializer.Serialize(writer, item, item.GetType(), options);
                }
                idx++;
            }
            writer.WriteEndObject();
        }
    }
}
