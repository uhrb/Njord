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
    public sealed class JsonStaticDataReportMessageConverter : JsonConverter<StaticDataReportMessage>
    {
        public override StaticDataReportMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);
            var userId = element.GetProperty("UserID").GetInt32().ToMMSIFormattedString();
            var messageId = JsonCheckedNumberEnumConverter<AisMessageType>.ConvertWithCheck(element.GetProperty("MessageID").GetInt32());
            var repeatIndicator = JsonCheckedNumberEnumConverter<RepeatIndicator>.ConvertWithCheck(element.GetProperty("RepeatIndicator").GetInt32());
            var partNumber = element.GetProperty("PartNumber").GetBoolean();
            string? name = null;
            string? callSign = null;
            IDimensions? dims = null;
            PositionFixingDeviceType? typeOfFix = null;
            TypeOfShipAndCargoType? typeOfShipAndCargoType = null;
            string? vendorIdName = null;
            byte? vendorIdModel = null;
            uint? vendorIdSerial = null;
            if (false == partNumber)
            {
                // PART A
                name = element.GetProperty("ReportA").GetProperty("Name").GetString()?.Trim();
            }
            else
            {
                var partB = element.GetProperty("ReportB");
                callSign = partB.GetProperty("CallSign").GetString()?.Trim();
                dims = JsonSerializer.Deserialize<Dimensions>(partB.GetProperty("Dimension"), options);
                typeOfFix = JsonCheckedNumberEnumConverter<PositionFixingDeviceType>.ConvertWithCheck(partB.GetProperty("FixType").GetInt32());
                typeOfShipAndCargoType = JsonCheckedNumberEnumConverter<TypeOfShipAndCargoType>.ConvertWithCheck(partB.GetProperty("ShipType").GetInt32());
                vendorIdModel = partB.GetProperty("VenderIDModel").GetByte();
                vendorIdSerial = partB.GetProperty("VenderIDSerial").GetUInt32();
                vendorIdName = partB.GetProperty("VendorIDName").GetString();
            }

            return new StaticDataReportMessage
            {
                UserId = userId,
                MessageId = messageId,
                RepeatIndicator = repeatIndicator,
                IsPartA = partNumber == false,
                Name = name,
                CallSign = callSign,
                Dimensions = dims,
                TypeOfShipAndCargoType = typeOfShipAndCargoType ?? TypeOfShipAndCargoType.NotAvailable,
                FixingDeviceType = typeOfFix ?? PositionFixingDeviceType.Undefined,
                ManufacturerId = vendorIdName,
                UnitModelCode = vendorIdModel,
                UnitSerialNumber = vendorIdSerial
            };

        }

        public override void Write(Utf8JsonWriter writer, StaticDataReportMessage value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("UserID", int.Parse(value.UserId));
            writer.WriteNumber("MessageID", (byte)value.MessageId);
            writer.WriteNumber("RepeatIndicator", (byte)value.RepeatIndicator);
            writer.WriteBoolean("PartNumber",  !value.IsPartA);

            writer.WritePropertyName("ReportA");
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteEndObject();

            writer.WritePropertyName("ReportB");
            writer.WriteStartObject();
            writer.WriteString("CallSign", value.CallSign);
            var valueToWrite = value.Dimensions ?? new Dimensions { A = 0, B = 0, C = 0, D = 0 };
            writer.WritePropertyName("Dimension");
            new JsonInterfaceFactoryConverter<IDimensions, Dimensions>().Write(writer, valueToWrite, options);
            writer.WriteNumber("FixType", (byte)value.FixingDeviceType);
            writer.WriteNumber("ShipType", (byte)value.TypeOfShipAndCargoType);
            writer.WriteNumber("VenderIDModel", value.UnitModelCode ?? 0);
            writer.WriteNumber("VenderIDSerial", value.UnitSerialNumber ?? 0);
            writer.WriteString("VendorIDName", value.ManufacturerId);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
