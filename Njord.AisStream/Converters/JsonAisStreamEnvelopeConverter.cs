using Njord.Ais.Interfaces;
using Njord.AisStream.Messages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Converters
{
    public class JsonAisStreamEnvelopeConverter : JsonConverter<AisStreamEnvelope>
    {
        public override AisStreamEnvelope? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);
            var prop = element.GetProperty("MetaData");
            var meta = JsonSerializer.Deserialize<AisStreamMetadata>(prop, options) ?? throw new JsonException("Malformed message without metadata specified");
            var messageTypeString = element.GetProperty("MessageType").GetString();
            if (string.IsNullOrWhiteSpace(messageTypeString))
            {
                throw new JsonException("Maformed message without MessageType specified");
            }

            var messageType = Enum.Parse<AisStreamMessageType>(messageTypeString);
            var messageProperty = element.GetProperty("Message").GetProperty(messageTypeString);

            IMessageId? messageDecoded = null;

            switch (messageType)
            {
                case AisStreamMessageType.AddressedBinaryMessage:
                    messageDecoded = JsonSerializer.Deserialize<AddressedBinaryMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.AddressedSafetyMessage:
                    messageDecoded = JsonSerializer.Deserialize<AddressedSafetyMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.AidsToNavigationReport:
                    messageDecoded = JsonSerializer.Deserialize<AidsToNavigationReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.AssignedModeCommand:
                    messageDecoded = JsonSerializer.Deserialize<AssignedModeCommandMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.BaseStationReport:
                    messageDecoded = JsonSerializer.Deserialize<BaseStationReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.BinaryAcknowledge:
                    messageDecoded = JsonSerializer.Deserialize<BinaryAcknowledgeMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.ChannelManagement:
                    messageDecoded = JsonSerializer.Deserialize<ChannelManagementMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.CoordinatedUTCInquiry:
                    messageDecoded = JsonSerializer.Deserialize<CoordinatedUniversalTimeDateInquiryMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.DataLinkManagementMessage:
                    messageDecoded = JsonSerializer.Deserialize<DataLinkManagementMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.ExtendedClassBPositionReport:
                    messageDecoded = JsonSerializer.Deserialize<ExtendedClassBEquipmentPositionReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.GnssBroadcastBinaryMessage:
                    messageDecoded = JsonSerializer.Deserialize<GnssBroadcastBinaryMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.Interrogation:
                    messageDecoded = JsonSerializer.Deserialize<InterrogationMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.LongRangeAisBroadcastMessage:
                    messageDecoded = JsonSerializer.Deserialize<LongRangeAisBroadcastMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.MultiSlotBinaryMessage:
                    messageDecoded = JsonSerializer.Deserialize<MultislotBinaryMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.PositionReport:
                    messageDecoded = JsonSerializer.Deserialize<PositionReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.SafetyBroadcastMessage:
                    messageDecoded = JsonSerializer.Deserialize<SafetyRelatedBroadcastMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.GroupAssignmentCommand:
                    messageDecoded = JsonSerializer.Deserialize<GroupAssignmentCommandMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.ShipStaticData:
                    messageDecoded = JsonSerializer.Deserialize<ShipStaticAndVoyageRelatedDataMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.SingleSlotBinaryMessage:
                    messageDecoded = JsonSerializer.Deserialize<SingleSlotBinaryMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.StandardClassBPositionReport:
                    messageDecoded = JsonSerializer.Deserialize<StandardClassBEquipmentReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.StandardSearchAndRescueAircraftReport:
                    messageDecoded = JsonSerializer.Deserialize<StandardSARAircraftPositionReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.StaticDataReport:
                    messageDecoded = JsonSerializer.Deserialize<StaticDataReportMessage>(messageProperty, options);
                    break;
                case AisStreamMessageType.UnknownMessage:
                    // nothing
                    break;
                default:
                    throw new JsonException("Unknown AisStreamMessageType");
            }

            return new AisStreamEnvelope { Message = messageDecoded, MessageType = messageType, Metadata = meta };
        }

        public override void Write(Utf8JsonWriter writer, AisStreamEnvelope value, JsonSerializerOptions options)
        {
            var messageTypeString = Enum.GetName(value.MessageType);
            if (string.IsNullOrEmpty(messageTypeString))
            {
                throw new JsonException("Message type is invalid");
            }
            writer.WriteStartObject();
            writer.WritePropertyName("MetaData");
            JsonSerializer.Serialize(writer, value.Metadata, options);
            writer.WriteString("MessageType", messageTypeString);

            writer.WritePropertyName("Message");
            writer.WriteStartObject();
            writer.WritePropertyName(messageTypeString);
            if (value.MessageType != AisStreamMessageType.UnknownMessage && value.Message != null)
            {
                JsonSerializer.Serialize(writer, value.Message, value.Message.GetType(), options);
            }
            writer.WriteEndObject();

            writer.WriteEndObject(); // envelope
        }
    }
}
