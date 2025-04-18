using Njord.Ais.Enums;

namespace Njord.Ais.Extensions.Types
{
    public static class MMSIStringExtensions
    {
        public static bool IsValidMMSI(this string mmsi)
        {
            if (string.IsNullOrEmpty(mmsi))
            {
                return false;
            }

            if (int.TryParse(mmsi, out int result))
            {
                if (result > 0 && result <= 999999999)
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToMMSIFormattedString(this int number)
        {
            return number.ToString().PadLeft(9, '0');
        }

        public static MaritimeEntityType ToMaritimeEntityType(this string mmsi)
        {
            if (false == mmsi.IsValidMMSI())
            {
                return MaritimeEntityType.Unknown;
            }

            return mmsi[0] switch
            {
                '0' => mmsi[1] switch
                {
                    '0' => mmsi[5] switch
                    {
                        '1' => MaritimeEntityType.CoastStation,// 00MID1XXX - Coast station
                        '2' => MaritimeEntityType.PortStation,// 00MID2XXX - Port station
                        '3' => MaritimeEntityType.PilotStation,// 00MID3XXX - Pilot station
                        '4' => MaritimeEntityType.RepeaterStation,// 00MID4XXX - AIS repeater station
                        '5' => MaritimeEntityType.BaseStation,// 00MID5XXX - AIS base station
                        _ => MaritimeEntityType.Unknown,
                    },
                    _ => MaritimeEntityType.ShipStationGroup, // 0MIDXXXXX - group stations
                },
                '1' => mmsi[..3] switch
                {
                    "111" => mmsi[6] switch
                    {
                        '1' => MaritimeEntityType.SearchAndRescueFixedWingAircraft, // 111MID1XX - Fixed wing aircraft
                        '5' => MaritimeEntityType.SearchAndRescueHelicopter,// 111MID5XX - Helicopters
                        _ => MaritimeEntityType.Unknown
                    },
                    _ => MaritimeEntityType.Unknown,
                },
                '2' or '3' or '4' or '5' or '6' or '7' => MaritimeEntityType.Vessel,// MIDXXXXXX
                '8' => MaritimeEntityType.HandheldVHF,
                '9' => mmsi[1] switch
                {
                    '9' => mmsi[5] switch
                    {
                        '1' => MaritimeEntityType.PhysicalAidsToNavigation,// 99MID1XXX - Physical ATON
                        '6' => MaritimeEntityType.VirtualAidsToNavigation,// 99MID6XXX - Virtual ATON
                        '8' => MaritimeEntityType.MobileAidsToNavigation,// 99MID8XXX - Mobile ATON
                        _ => MaritimeEntityType.Unknown
                    },
                    '7' => mmsi[2] switch
                    {
                        '0' => MaritimeEntityType.SearchAndRescueTransmitter,// 970XXYYYY - Automatic identification system-search and rescue transmitter2
                        '2' => MaritimeEntityType.ManOverBoard,// 972XXYYYY - Man overboard
                        '4' => MaritimeEntityType.EmergencyPositionIdentificationSystem,// 974XXYYYY - Emergency position-indicating radio beacon-automatic identification system
                        '9' => MaritimeEntityType.AutonomousDeviceGroupB,// 979YYYYYY - Autonomous maritime radio devices Group B
                        _ => MaritimeEntityType.Unknown
                    },
                    _ => MaritimeEntityType.Unknown
                },
                _ => MaritimeEntityType.Unknown,
            };
        }

    }
}
