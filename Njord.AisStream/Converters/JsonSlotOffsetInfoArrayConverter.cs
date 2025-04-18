using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Interfaces;
using Njord.AisStream.ModelTypes;
using System.Text.Json;

namespace Njord.AisStream.Converters
{
    public class JsonSlotOffsetInfoArrayConverter : JsonAisStreamArrayConverter<ISlotOffsetInfo, SlotOffsetInfo>
    {
        public override (ISlotOffsetInfo?, bool, bool) Deserialize(int index, JsonElement entry, JsonSerializerOptions options)
        {
            var (item, _, _)= base.Deserialize(index, entry, options);
            if (item?.IsInterrogatedButNoDatalinkInformationAvailiable() == true)
            {
                return (item, true, false);
            }

            return (item, true, true);
        }
    }
}
