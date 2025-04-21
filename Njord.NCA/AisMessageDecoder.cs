namespace Njord.NCA
{
    public class AisMessageDecoder
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



    }
}
