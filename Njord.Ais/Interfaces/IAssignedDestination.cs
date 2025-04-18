namespace Njord.Ais.Interfaces
{
    public interface IAssignedDestination : IDestinationId
    {
        /// <summary>
        /// Offset from current slot to first assigned slot
        /// Minimum - 37.5 when operating in the assigned mode using report rate assignment;
        /// Minimum 45 when operating in the assigned mode using slot increment assignment and the SOTDMA communication state.
        /// Also when operating in the assigned mode using SOTDMA as given by Table 46, Annex 8.
        /// To summ-up, slots needs to be more than 2.
        /// </summary>
        public ushort Offset { get;init; }

        /// <summary>
        /// Increment to next assigned slot
        /// To assign a reporting rate for a station, the parameter 
        /// increment should be set to zero. The parameter offset 
        /// should then be interpreted as the number of reports 
        /// in a time interval of 10 min.
        /// </summary>
        public ushort Increment { get;init; }
    }
}
