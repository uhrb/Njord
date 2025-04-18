using Njord.Ais.Interfaces;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.ModelTypes;
using System.Text.Json;

namespace Njord.AisStream.Converters
{
    public class JsonAisStreamAcknowledgedDestinationConverter : JsonAisStreamArrayConverter<IAcknowledgementDestination, AcknowledgementDestination>
    {
        public override (IAcknowledgementDestination?, bool, bool) Deserialize(int index, JsonElement entry, JsonSerializerOptions options)
        {
            var valid = entry.GetProperty("Valid").GetBoolean();
            if (!valid)
            {
                return (null, false, true);
            }

            return base.Deserialize(index, entry, options);
        }


        public override void Write(Utf8JsonWriter writer, IEnumerable<IAcknowledgementDestination> value, JsonSerializerOptions options)
        {
            var idx = 0;
            writer.WriteStartObject();
            if (!options.Converters.Any(_ => _.Type == typeof(IAcknowledgementDestination)))
            {
                options = new JsonSerializerOptions(options);
                options.Converters.Add(new JsonInterfaceFactoryConverter<IAcknowledgementDestination, AcknowledgementDestination>());
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
                    writer.WriteStartObject();
                    writer.WritePropertyName("Valid");
                    writer.WriteBooleanValue(true);
                    writer.WriteNumber("DestinationID", int.Parse(item.DestinationId));
                    writer.WriteNumber("Sequenceinteger", item.SequenceNumber);
                    writer.WriteEndObject();
                }
                idx++;
            }
            writer.WriteEndObject();
        }
    }
}
