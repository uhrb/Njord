using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.Messages;
using Njord.AisStream.ModelTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.AisStream.MessageConverters
{
    public class JsonGroupAssignmentCommandMessageConverter : JsonConverter<GroupAssignmentCommandMessage>
    {
        public override GroupAssignmentCommandMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);
            var stationType = JsonCheckedNumberEnumConverter<StationType>.ConvertWithCheck(element.GetProperty("StationType").GetInt32());
            var reportingInterval = JsonCheckedNumberEnumConverter<ReportingInterval>.ConvertWithCheck(element.GetProperty("ReportingInterval").GetInt32());
            var quiteTime = element.GetProperty("QuietTime").GetByte();
            var userId = element.GetProperty("UserID").GetInt32().ToMMSIFormattedString();
            var messageId = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(element.GetProperty("MessageID").GetInt32());
            var repeatIndicator = JsonCheckedNumberEnumConverter<RepeatIndicator>.ConvertWithCheck(element.GetProperty("RepeatIndicator").GetInt32());
            var shipType = JsonCheckedNumberEnumConverter<TypeOfShipAndCargoType>.ConvertWithCheck(element.GetProperty("ShipType").GetInt32());
            var txRx = JsonCheckedNumberEnumConverter<TxRxModeType>.ConvertWithCheck(element.GetProperty("TxRxMode").GetInt32());
            var area = new GeoArea
            {
                LatitudeLeftUp = element.GetProperty("Latitude1").GetDouble(),
                LongitudeLeftUp = element.GetProperty("Longitude1").GetDouble(),
                LatitudeRightDown = element.GetProperty("Latitude2").GetDouble(),
                LongitudeRightDown = element.GetProperty("Longitude2").GetDouble()
            };

            return new GroupAssignmentCommandMessage
            {
                GeoArea = area,
                MessageId = messageId,
                QuietTime = quiteTime,
                RepeatIndicator = repeatIndicator,
                ReportingInterval = reportingInterval,
                StationType = stationType,
                UserId = userId,
                TxRxMode = txRx,
                TypeOfShipAndCargoType = shipType,
            };
        }

        public override void Write(Utf8JsonWriter writer, GroupAssignmentCommandMessage value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("Latitude1", value.GeoArea.LatitudeLeftUp);
            writer.WriteNumber("Longitude1", value.GeoArea.LongitudeLeftUp);
            writer.WriteNumber("Latitude2", value.GeoArea.LatitudeRightDown);
            writer.WriteNumber("Longitude2", value.GeoArea.LongitudeRightDown);
            writer.WriteNumber("MessageID", (byte)value.MessageId);
            writer.WriteNumber("QuietTime", value.QuietTime);
            writer.WriteNumber("RepeatIndicator", (byte)value.RepeatIndicator);
            writer.WriteNumber("ReportingInterval", (byte)value.ReportingInterval);
            writer.WriteNumber("StationType", (byte)value.StationType);
            writer.WriteNumber("UserID", int.Parse(value.UserId));
            writer.WriteNumber("TxRxMode", (byte)value.TxRxMode);
            writer.WriteNumber("ShipType", (byte)value.TypeOfShipAndCargoType);
            writer.WriteEndObject();
        }
    }
}
