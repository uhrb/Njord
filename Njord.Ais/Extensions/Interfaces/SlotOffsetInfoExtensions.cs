using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class SlotOffsetInfoExtensions
    {
        public static bool IsValid(this ISlotOffsetInfo slotOffsetInfo)
        {
            return slotOffsetInfo.NumberOfSlots >= 0
                && slotOffsetInfo.NumberOfSlots <= 15
                && slotOffsetInfo.TimeoutMinutes < 8
                && slotOffsetInfo.Increment < 2048
                && slotOffsetInfo.SlotOffset < 4096;
        }

        public static bool IsInterrogatedButNoDatalinkInformationAvailiable(this ISlotOffsetInfo slotOffsetInfo)
        {
            return slotOffsetInfo.Increment == 0 && slotOffsetInfo.SlotOffset == 0 && slotOffsetInfo.NumberOfSlots == 0;
        }
    }
}
