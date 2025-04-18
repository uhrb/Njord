using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Tests
{
    public class CommunicationStateSOTDMATests
    {
        [Fact]
        public void DecodingEncodingWorksCorrectly()
        {
            for (byte hours = 0; hours < 24; hours++)
            {
                for (byte minutes = 0; minutes < 60; minutes++)
                {
                    var encoded = (new UtcHourMinute { Hour = hours, Minute = minutes }).HourMinuteToSubMessageSOTDMA();
                    var (decodedHour, decodedMinute)= (new CommunicationStateSOTDMA
                    {
                        SubMessage = encoded,
                        SlotTimeout = 0,
                        SyncState = 0
                    }).SubMessageToHourMinute();
                    Assert.Equal(decodedHour, hours);
                    Assert.Equal(decodedMinute, minutes);
                }
            }
        }


        [Theory]
        [InlineData([(ushort)0b_0001_1100_1111_0000, (byte)14, (byte)30])]
        public void EncodingWorksCorrectly(ushort submessage, byte expectedHours, byte expectedMinutes)
        {
            var structure = new UtcHourMinute { Hour = expectedHours, Minute = expectedMinutes };
            var encoded = structure.HourMinuteToSubMessageSOTDMA();
            Assert.Equal(submessage, encoded);
        }

        private record UtcHourMinute : IUtcHourMinute
        {
            public byte Hour { get; init; }
            public byte Minute { get; init; }
        }

        private record CommunicationStateSOTDMA : ICommunicationStateSOTDMA
        {
            public CommunicationSyncState SyncState { get; init; }
            public byte SlotTimeout { get; init; }
            public ushort SubMessage { get; init; }
        }
    }
}
