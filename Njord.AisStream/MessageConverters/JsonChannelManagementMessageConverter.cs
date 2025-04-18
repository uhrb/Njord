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
    public class JsonChannelManagementMessageConverter : JsonConverter<ChannelManagementMessage>
    {
        public override ChannelManagementMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);
            var userId = element.GetProperty("UserID").GetInt32().ToMMSIFormattedString();
            var messageId = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(element.GetProperty("MessageID").GetInt32());
            var repeatIndicator = JsonCheckedNumberEnumConverter<RepeatIndicator>.ConvertWithCheck(element.GetProperty("RepeatIndicator").GetInt32());
            var channelA = element.GetProperty("ChannelA").GetUInt16();
            var channelB = element.GetProperty("ChannelB").GetUInt16();
            var txrx = JsonCheckedNumberEnumConverter<TxRxModeType>.ConvertWithCheck(element.GetProperty("TxRxMode").GetInt32());
            var lowPower = element.GetProperty("LowPower").GetBoolean();
            var tranZone = element.GetProperty("TransitionalZoneSize").GetByte();
            var isAddressed = element.GetProperty("IsAddressed").GetBoolean();
            var bwa = element.GetProperty("BwA").GetBoolean();
            var bwb = element.GetProperty("BwB").GetBoolean();
            IGeoArea? geoArea = null;
            IEnumerable<string>? stations = null;

            if (isAddressed)
            {
                var areaElement = element.GetProperty("Unicast");
                var station1 = areaElement.GetProperty("AddressStation1").GetInt32();
                var station2 = areaElement.GetProperty("AddressStation2").GetInt32();
                var toRet = new List<string>(2);
                if (station1 != 0)
                {
                    toRet.Add(station1.ToMMSIFormattedString());
                    if (station2 != 0)
                    {
                        toRet.Add(station2.ToMMSIFormattedString());
                    }
                }
                stations = [.. toRet];
            }
            else
            {
                geoArea = JsonSerializer.Deserialize<GeoArea>(element.GetProperty("Area"), options);
            }

            return new ChannelManagementMessage
            {
                UserId = userId,
                MessageId = messageId,
                RepeatIndicator = repeatIndicator,
                ChannelA = channelA,
                ChannelB = channelB,
                TxRxMode = txrx,
                IsLowPower = lowPower,
                TransitionalZoneSize = tranZone,
                IsAddressed = isAddressed,
                IsChannelABandwidthSpare = bwa,
                IsChannelBBandwidthSpare = bwb,
                Area = geoArea,
                AddressedStations = stations
            };
        }

        public override void Write(Utf8JsonWriter writer, ChannelManagementMessage value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteNumber("UserID", int.Parse(value.UserId));
            writer.WriteNumber("MessageID", (byte)value.MessageId);
            writer.WriteNumber("RepeatIndicator", (byte)value.RepeatIndicator);
            writer.WriteNumber("ChannelA", value.ChannelA);
            writer.WriteNumber("ChannelB", value.ChannelB);
            writer.WriteNumber("TxRxMode", (byte)value.TxRxMode);
            writer.WriteBoolean("LowPower", value.IsLowPower);
            writer.WriteNumber("TransitionalZoneSize", value.TransitionalZoneSize);
            writer.WriteBoolean("IsAddressed", value.IsAddressed);
            writer.WriteBoolean("BwA", value.IsChannelABandwidthSpare);
            writer.WriteBoolean("BwB", value.IsChannelBBandwidthSpare);
            writer.WritePropertyName("Area");
            var valueToWrite = value.Area ?? new GeoArea();
            new JsonInterfaceFactoryConverter<IGeoArea, GeoArea>().Write(writer, valueToWrite, options);
           
            string?[] stations = [null, null];
            var idx = 0;
            if (value.AddressedStations != null)
            {
                foreach (var item in value.AddressedStations)
                {
                    if (idx > 1)
                    {
                        break;
                    }
                    stations[idx] = item;

                    idx++;
                }

            }
            writer.WritePropertyName("Unicast");
            writer.WriteStartObject();
            idx = 1;
            foreach(var station in stations)
            {
                writer.WriteNumber($"AddressedStation{idx}", station == null ? 0 : int.Parse(station));
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
