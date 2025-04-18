using Njord.Ais.Enums;
using Njord.Ais.Extensions.Messages;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class Vessel : AbstractMaritimeGrain, IVessel
    {
        private readonly IPersistentState<VesselState> _state;

        public Vessel([PersistentState(nameof(Vessel))] IPersistentState<VesselState> state, ILogger<Vessel> logger) : base(logger)
        {
            _state = state;

            Map(_ => ProcessPositionReport((IPositionReportMessage)_),
                AisMessageType.PositionReportScheduled,
                AisMessageType.PositionReportAssignedScheduled,
                AisMessageType.PositionReportSpecial);

            Map(_ => ProcessStandardClassBEquipmentReportMessage((IStandardClassBEquipmentReportMessage)_), AisMessageType.StandardClassBPositionReport);

            Map(_ => ProcessExtendedClassBPositionReportMessage((IExtendedClassBEquipmentPositionReportMessage)_), AisMessageType.ExtendedClassBPositionReport);

            Map(_ => ProcessLongRangeAisBroadcastMessage((ILongRangeAisBroadcastMessage)_), AisMessageType.LongRangeAisBroadcastMessage);

            Map(_ => ProcessBaseStationReportMobileMessage((IBaseStationReportMessage)_), AisMessageType.CoordinatedUtcBaseStationReport);

            Map(_ => ProcessShipStaticAndVoyageRelatedDataMessage((IShipStaticAndVoyageRelatedDataMessage)_), AisMessageType.ShipStaticAndVoyageRelatedData);

            Map(_ => ProcessShipStaticDataMessage((IStaticDataReportMessage)_), AisMessageType.StaticDataReport);

            Map(ProcessCommunication,
                AisMessageType.BinaryAcknowledge,
                AisMessageType.BinaryBroadcast,
                AisMessageType.AddressedSafetyMessage,
                AisMessageType.BinaryAcknowledgeSafety,
                AisMessageType.SafetyBroadcastMessage,
                AisMessageType.Interrogation,
                AisMessageType.SingleSlotBinaryMessage,
                AisMessageType.MultiSlotBinaryMessage);
        }

        private async Task ProcessShipStaticDataMessage(IStaticDataReportMessage message)
        {
            if (false == message.IsValid()) return;

            if (message.IsPartA)
            {
                _state.State.Name = message.Name;
            }
            else
            {
                _state.State.CallSign = message.CallSign;
                _state.State.Dimensions = message.Dimensions;
                _state.State.FixingDeviceType = message.FixingDeviceType;
                _state.State.TypeOfShipAndCargoType = message.TypeOfShipAndCargoType;
            }

            await _state.WriteStateAsync();
        }

        private async Task ProcessBaseStationReportMobileMessage(IBaseStationReportMessage message)
        {
            if (false == message.IsValid()) return;

            _state.State.FixingDeviceType = message.FixingDeviceType;
            _state.State.IsPositionAccuracyHigh = message.IsPositionAccuracyHigh;
            _state.State.IsRaimInUse = message.IsRaimInUse;
            _state.State.Latitude = message.Latitude;
            _state.State.Longitude = message.Longitude;

            await _state.WriteStateAsync();
        }

        private async Task ProcessLongRangeAisBroadcastMessage(ILongRangeAisBroadcastMessage message)
        {
            if (false == message.IsValid()) return;

            _state.State.NavigationalStatus = message.NavigationalStatus;
            _state.State.Latitude = message.Latitude;
            _state.State.Longitude = message.Longitude;
            _state.State.CourseOverGround = message.CourseOverGround;
            _state.State.SpeedOverGround = message.SpeedOverGround;
            _state.State.IsPositionAccuracyHigh = message.IsPositionAccuracyHigh;

            await _state.WriteStateAsync();
        }

        private async Task ProcessExtendedClassBPositionReportMessage(IExtendedClassBEquipmentPositionReportMessage message)
        {
            if (false == message.IsValid()) return;

            _state.State.TrueHeading = message.TrueHeading;
            _state.State.FixingDeviceType = message.FixingDeviceType;
            _state.State.IsPositionAccuracyHigh = message.IsPositionAccuracyHigh;
            _state.State.TypeOfShipAndCargoType = message.TypeOfShipAndCargoType;
            _state.State.SpeedOverGround = message.SpeedOverGround;
            _state.State.CourseOverGround = message.CourseOverGround;
            _state.State.Dimensions = message.Dimensions;
            _state.State.IsRaimInUse = message.IsRaimInUse;
            _state.State.Latitude = message.Latitude;
            _state.State.Longitude = message.Longitude;
            _state.State.Name = message.Name;

            await _state.WriteStateAsync();
        }

        private async Task ProcessShipStaticAndVoyageRelatedDataMessage(IShipStaticAndVoyageRelatedDataMessage message)
        {
            if (false == message.IsValid()) return;

            _state.State.CallSign = message.CallSign;
            _state.State.Destination = message.Destination;
            _state.State.IMONumber = message.IMONumber;
            _state.State.MaximumPresentStaticDraught = message.MaximumPresentStaticDraught;
            _state.State.EstimatedTimeOfArrival = message.EstimatedTimeOfArrival;
            _state.State.Dimensions = message.Dimensions;
            _state.State.FixingDeviceType = message.FixingDeviceType;
            _state.State.Name = string.IsNullOrEmpty(message.Name) ? _state.State.Name : message.Name;
            _state.State.TypeOfShipAndCargoType = message.TypeOfShipAndCargoType;

            await _state.WriteStateAsync();
        }

        private async Task ProcessStandardClassBEquipmentReportMessage(IStandardClassBEquipmentReportMessage message)
        {
            if (false == message.IsValid()) return;

            _state.State.SpeedOverGround = message.SpeedOverGround;
            _state.State.CourseOverGround = message.CourseOverGround;
            _state.State.Longitude = message.Longitude;
            _state.State.Latitude = message.Latitude;
            _state.State.IsPositionAccuracyHigh = message.IsPositionAccuracyHigh;
            _state.State.IsRaimInUse = message.IsRaimInUse;

            await _state.WriteStateAsync();
        }

        private async Task ProcessPositionReport(IPositionReportMessage message)
        {
            if (false == message.IsValid()) return;

            UpdateFromMovingPositionMessage(message, _state);

            _state.State.RateOfTurn = message.RateOfTurn;
            _state.State.SpecialManoeuvreIndicator = message.SpecialManoeuvreIndicator;
            _state.State.TrueHeading = message.TrueHeading;
            _state.State.NavigationalStatus = message.NavigationalStatus;

            await _state.WriteStateAsync();
        }

        private Task ProcessCommunication(IMessageId id)
        {
            return Task.CompletedTask;
        }
    }
}
