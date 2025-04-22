using System.Text;

namespace Njord.NCA
{
    public class NmeaMessageDecoder
    {
        // <format>,<message count>,<message id>,<sequence id>,<channel A/B>,<data>,<fill bits>
        // Words 2,3 and 4 are the no of parts, this part and a part id
        // The 5th word is the AIS radio channel which has been used for transmission.
        // 6th word is AIS payload
        // AIS messages are contained entirely in the payload (6th) word of a NMEA sentence or in the
        // case of multi-part messages in multiple payloads, after re-assembly by the decoder.
        // The messages, of which there are 27 basic types, are split into separate fields.
        // Each field contains a number of bits. These fields are also encoded as they could represent numerical data (eg speed)
        // or textual data (eg vessel's name) or require conversion from a numeric code
        // to a meaningful description (eg type of vessel where 50 = Pilot Vessel). AisDecoder decodes around 1500 separate fields.
        // The first 3 fields of AIS messages are always Message type, Repeat indicator and MMSI.
        // !BSVDM,1,1,,A,33o6t@50001;upbWFlOc<pLh0DQJ,0*6C
        // !AIVDM, !AIVDO, !BSVDM, !ABVDM
        // The first word !AIVDM is the name of the sentence, the last 2 characters are the checksum.
        // \s:2573300,c:1745314223*0E\!BSVDM,2,1,1,B,53omwv81t0j@h=0L000Pu1<Hu8@0000000000166?5658A5:hRCQlh@000,0*0A
        // c: 1745314223 - unix timestamp

        public void DecodeFromString(string nmea, Encoding enc)
        {
            var parts = nmea.Split(',');
            var messageForm = parts[0];
            var messageCount = int.Parse(parts[1]);
            var messageId = int.Parse(parts[2]);
            var sequenceId = int.Parse(parts[3]);
            var channel = parts[4];
            var data = parts[5];
            var checkSum = parts[6];
            var bytes = enc.GetBytes(data);
            var aisMessageId = bytes[0] & 0_0011_1111;
            var repeatIndicator = bytes[0] & 0_1100_0000;
        }
    }
}
