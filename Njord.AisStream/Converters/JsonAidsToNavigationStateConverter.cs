using Njord.Ais.Extensions.Types;
using Njord.AisStream.ModelTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Converters
{
    public class JsonAidsToNavigationStateConverter : JsonConverter<AidsToNavigationState>
    {
        public override AidsToNavigationState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var (racon, light, alarm) = reader.GetByte().DecodeAidsToNavigationStatusFromByte();
            return new AidsToNavigationState
            {
                IsAlarmState = alarm,
                LightsState = light,
                RACONState = racon
            };
        }

        public override void Write(Utf8JsonWriter writer, AidsToNavigationState value, JsonSerializerOptions options)
        {
            var bytes = value.EncodeAidsToNavigationStatusToByte();
            writer.WriteNumberValue(bytes);
        }
    }
}
