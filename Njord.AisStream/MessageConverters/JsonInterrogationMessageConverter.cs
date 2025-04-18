using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Interfaces;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.Messages;
using Njord.AisStream.ModelTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.AisStream.MessageConverters
{
    public class JsonInterrogationMessageConverter : JsonConverter<InterrogationMessage>
    {
        public override InterrogationMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);
            var userId = element.GetProperty("UserID").GetInt32().ToMMSIFormattedString();
            var messageId = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(element.GetProperty("MessageID").GetInt32());
            var repeat = JsonCheckedNumberEnumConverter<RepeatIndicator>.ConvertWithCheck(element.GetProperty("RepeatIndicator").GetInt32());
            return new InterrogationMessage
            {
                Interrogations = BuildInterrogations(element),
                MessageId = messageId,
                RepeatIndicator = repeat,
                UserId = userId
            };

        }

        private static IEnumerable<IInterrogationWithDestination> BuildInterrogations(JsonElement element)
        {
            /*
             * Skipping because of AisStream BUG https://github.com/aisstream/issues/issues/119
            if (element.TryGetProperty("Station1Msg1", out JsonElement station1Msg1))
            {
                var stationId = station1Msg1.GetProperty("StationID").GetInt32();
                var messageType = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(station1Msg1.GetProperty("MessageID").GetInt32());
                var stationIdString = stationId.ToMMSIFormattedString();
                yield return new InterrogationWithDestination
                {
                    DestinationId = stationIdString,
                    MessageType = messageType,
                    SlotOffset = station1Msg1.GetProperty("SlotOffset").GetUInt16()
                };

                if (element.TryGetProperty("Station1Msg2", out JsonElement station1Msg2))
                {
                    var secondValid = station1Msg2.GetProperty("Valid").GetBoolean();
                    if (secondValid)
                    {
                        yield return new InterrogationWithDestination
                        {
                            DestinationId = stationIdString,
                            MessageType = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(station1Msg2.GetProperty("MessageID").GetInt32()),
                            SlotOffset = station1Msg2.GetProperty("SlotOffset").GetUInt16()
                        };
                    }
                }
            }
            */

            if (element.TryGetProperty("Station2", out JsonElement station2))
            {
                var thridValid = station2.GetProperty("Valid").GetBoolean();
                if (thridValid)
                {
                    yield return new InterrogationWithDestination
                    {
                        DestinationId = station2.GetProperty("StationID").GetInt32().ToMMSIFormattedString(),
                        MessageType = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(station2.GetProperty("MessageID").GetInt32()),
                        SlotOffset = station2.GetProperty("SlotOffset").GetUInt16()
                    };
                }
            }

            yield break;
        }

        public override void Write(Utf8JsonWriter writer, InterrogationMessage value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("MessageID", (byte)value.MessageId);
            writer.WriteNumber("UserID", int.Parse(value.UserId));
            writer.WriteNumber("RepeatIndicator", (byte)value.RepeatIndicator);
            IInterrogationWithDestination?[] dests = [null, null, null];
            var idx = 0;
            if (value.Interrogations != null)
            {
                foreach (var item in value.Interrogations)
                {
                    if (idx > 2)
                    {
                        break;
                    }
                    dests[idx] = item;
                    idx++;
                }
            }

            writer.WritePropertyName("Station1Msg1");
            writer.WriteStartObject();
            writer.WriteNumber("MessageID", ((byte?)dests[0]?.MessageType) ?? 0);
            writer.WriteNumber("StationID", (int.Parse(dests[0]?.DestinationId ?? "0")));
            writer.WriteNumber("SlotOffset", dests[0]?.SlotOffset ?? 0);
            writer.WriteBoolean("Valid", dests[0] != null);
            writer.WriteEndObject();

            writer.WritePropertyName("Station1Msg2");
            writer.WriteStartObject();
            writer.WriteNumber("MessageID", ((byte?)dests[1]?.MessageType) ?? 0);
            writer.WriteNumber("SlotOffset", dests[1]?.SlotOffset ?? 0);
            writer.WriteBoolean("Valid", dests[1] != null);
            writer.WriteEndObject();

            writer.WritePropertyName("Station2");
            writer.WriteStartObject();
            writer.WriteNumber("MessageID", ((byte?)dests[2]?.MessageType) ?? 0);
            writer.WriteNumber("StationID", (int.Parse(dests[2]?.DestinationId ?? "0")));
            writer.WriteNumber("SlotOffset", dests[2]?.SlotOffset ?? 0);
            writer.WriteBoolean("Valid", dests[2] != null);
            writer.WriteEndObject();


            writer.WriteEndObject();
        }
    }
}
