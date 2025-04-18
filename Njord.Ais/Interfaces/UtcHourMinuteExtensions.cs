namespace Njord.Ais.Interfaces
{
    public static class UtcHourMinuteExtensions
    {
        /// <summary>
        /// Encodes UTC Hour and Minute to Submessage 
        /// </summary>
        /// <returns>Encoded value</returns>
        public static ushort HourMinuteToSubMessageSOTDMA(this IUtcHourMinute hourMinute)
        {
            var hoursMapped = _hoursDefinitionsForSubmessage[hourMinute.Hour];
            var minutesMapped = _minutesDefinitionsForSubmessage[hourMinute.Minute];
            return (ushort)(hoursMapped | minutesMapped);
        }

        /// <summary>
        /// Decodes submessage to UTC Hour and Minute
        /// </summary>
        /// <returns>Decoded hour and minute values</returns>
        public static (ushort, ushort) SubMessageToHourMinute(this ICommunicationStateSOTDMA communication)
        {
            var minutes = communication.SubMessage & 0b_0000_0001_1111_1100;
            var hours = communication.SubMessage & 0b_0011_1110_0000_0000;
            return ((byte)Array.IndexOf(_hoursDefinitionsForSubmessage, (ushort)hours), (byte)Array.IndexOf(_minutesDefinitionsForSubmessage, (ushort)minutes));
        }

        /// <summary>
        /// Mappings for ushort values for hours in SubMessage. Index correspond to hour 0-23
        /// </summary>
        private static readonly ushort[] _hoursDefinitionsForSubmessage = [
                  0b_0000_0000_0000_0000 , // 0
                  0b_0010_0000_0000_0000 , // 1
                  0b_0001_0000_0000_0000 , // 2
                  0b_0011_0000_0000_0000 , // 3
                  0b_0000_1000_0000_0000 , // 4
                  0b_0010_1000_0000_0000 , // 5
                  0b_0001_1000_0000_0000 , // 6
                  0b_0011_1000_0000_0000 , // 7
                  0b_0000_0100_0000_0000 , // 8
                  0b_0010_0100_0000_0000 , // 9
                  0b_0001_0100_0000_0000 , // 10
                  0b_0011_0100_0000_0000 , // 11
                  0b_0000_1100_0000_0000 , // 12
                  0b_0010_1100_0000_0000 , // 13
                  0b_0001_1100_0000_0000 , // 14
                  0b_0011_1100_0000_0000 , // 15
                  0b_0000_0010_0000_0000 , // 16
                  0b_0010_0010_0000_0000 , // 17
                  0b_0001_0010_0000_0000 , // 18
                  0b_0011_0010_0000_0000 , // 19
                  0b_0000_1010_0000_0000 , // 20
                  0b_0010_1010_0000_0000 , // 21
                  0b_0001_1010_0000_0000 , // 22
                  0b_0011_1010_0000_0000  // 23
        ];

        /// <summary>
        /// Mappings for ushort values for minutes in SubMessage. Index correspond to minute 0-59
        /// </summary>
        private static readonly ushort[] _minutesDefinitionsForSubmessage = [
                 0b_0000_0000_0000_0000 , // 0
                 0b_0000_0001_0000_0000 , // 1
                 0b_0000_0000_1000_0000 , // 2
                 0b_0000_0001_1000_0000 , // 3
                 0b_0000_0000_0100_0000 , // 4
                 0b_0000_0001_0100_0000 , // 5
                 0b_0000_0000_1100_0000 , // 6
                 0b_0000_0001_1100_0000 , // 7
                 0b_0000_0000_0010_0000 , // 8
                 0b_0000_0001_0010_0000 , // 9
                 0b_0000_0000_1010_0000 , // 10
                 0b_0000_0001_1010_0000 , // 11
                 0b_0000_0000_0110_0000 , // 12
                 0b_0000_0001_0110_0000 , // 13
                 0b_0000_0000_1110_0000 , // 14
                 0b_0000_0001_1110_0000 , // 15
                 0b_0000_0000_0001_0000 , // 16
                 0b_0000_0001_0001_0000 , // 17
                 0b_0000_0000_1001_0000 , // 18
                 0b_0000_0001_1001_0000 , // 19
                 0b_0000_0000_0101_0000 , // 20
                 0b_0000_0001_0101_0000 , // 21
                 0b_0000_0000_1101_0000 , // 22
                 0b_0000_0001_1101_0000 , // 23
                 0b_0000_0000_0011_0000 , // 24
                 0b_0000_0001_0011_0000 , // 25
                 0b_0000_0000_1011_0000 , // 26
                 0b_0000_0001_1011_0000 , // 27
                 0b_0000_0000_0111_0000 , // 28
                 0b_0000_0001_0111_0000 , // 29
                 0b_0000_0000_1111_0000 , // 30
                 0b_0000_0001_1111_0000 , // 31
                 0b_0000_0000_0000_1000 , // 32
                 0b_0000_0001_0000_1000 , // 33
                 0b_0000_0000_1000_1000 , // 34
                 0b_0000_0001_1000_1000 , // 35
                 0b_0000_0000_0100_1000 , // 36
                 0b_0000_0001_0100_1000 , // 37
                 0b_0000_0000_1100_1000 , // 38
                 0b_0000_0001_1100_1000 , // 39
                 0b_0000_0000_0010_1000 , // 40
                 0b_0000_0001_0010_1000 , // 41
                 0b_0000_0000_1010_1000 , // 42
                 0b_0000_0001_1010_1000 , // 43
                 0b_0000_0000_0110_1000 , // 44
                 0b_0000_0001_0110_1000 , // 45
                 0b_0000_0000_1110_1000 , // 46
                 0b_0000_0001_1110_1000 , // 47
                 0b_0000_0000_0001_1000 , // 48
                 0b_0000_0001_0001_1000 , // 49
                 0b_0000_0000_1001_1000 , // 50
                 0b_0000_0001_1001_1000 , // 51
                 0b_0000_0000_0101_1000 , // 52
                 0b_0000_0001_0101_1000 , // 53
                 0b_0000_0000_1101_1000 , // 54
                 0b_0000_0001_1101_1000 , // 55
                 0b_0000_0000_0011_1000 , // 56
                 0b_0000_0001_0011_1000 , // 57
                 0b_0000_0000_1011_1000 , // 58
                 0b_0000_0001_1011_1000  // 59
            ];
    }
}
