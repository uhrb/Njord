using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface IRepeatIndicator
    {

        /// <summary>
        /// Repeat indicator <see cref="Enums.RepeatIndicator"/>
        /// </summary>
        public RepeatIndicator RepeatIndicator { get; init; }
    }
}
