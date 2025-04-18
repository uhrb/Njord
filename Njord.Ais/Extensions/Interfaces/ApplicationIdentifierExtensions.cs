using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    /// <summary>
    /// Provides extension methods for the <see cref="IApplicationIdentifier"/> class.
    /// </summary>
    public static class ApplicationIdentifierExtensions
    {
        /// <summary>
        /// Determines whether the specified application identifier is an international designated area code.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns><c>true</c> if the designated area code is international; otherwise, <c>false</c>.</returns>
        public static bool IsInternationalDesignatedAreaCode(this IApplicationIdentifier appId)
        {
            return appId.DesignatedAreaCode == 1;
        }

        /// <summary>
        /// Determines whether the specified application identifier is a text telegram.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns><c>true</c> if the designated area code is 1 and the function identifier is 0; otherwise, <c>false</c>.</returns>
        public static bool IsTextTelegram(this IApplicationIdentifier appId)
        {
            return appId.DesignatedAreaCode == 1 && appId.FunctionIdentifier == 0;
        }

        /// <summary>
        /// Determines whether the specified application identifier is an interrogation for a specific function.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns><c>true</c> if the designated area code is 1 and the function identifier is 2; otherwise, <c>false</c>.</returns>
        public static bool IsInterrogationForSpecificFunction(this IApplicationIdentifier appId)
        {
            return appId.DesignatedAreaCode == 1 && appId.FunctionIdentifier == 2;
        }

        /// <summary>
        /// Determines whether the specified application identifier is a capability interrogation.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns><c>true</c> if the designated area code is 1 and the function identifier is 3; otherwise, <c>false</c>.</returns>
        public static bool IsCapabilityInterrogation(this IApplicationIdentifier appId)
        {
            return appId.DesignatedAreaCode == 1 && appId.FunctionIdentifier == 3;
        }

        /// <summary>
        /// Determines whether the specified application identifier is a capability interrogation reply.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns><c>true</c> if the designated area code is 1 and the function identifier is 4; otherwise, <c>false</c>.</returns>
        public static bool IsCapabilityInterrogationReply(this IApplicationIdentifier appId)
        {
            return appId.DesignatedAreaCode == 1 && appId.FunctionIdentifier == 4;
        }

        /// <summary>
        /// Determines whether the specified application identifier is an application acknowledgement to an addressed binary message.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns><c>true</c> if the designated area code is 1 and the function identifier is 5; otherwise, <c>false</c>.</returns>
        public static bool IsApplicationAcknowledgementToAddressedBinaryMessage(this IApplicationIdentifier appId)
        {
            return appId.DesignatedAreaCode == 1 && appId.FunctionIdentifier == 5;
        }

        /// <summary>
        /// Checks if Application identifier is formally valid
        /// </summary>
        /// <param name="appid">App id to check</param>
        /// <returns>True if formally valid</returns>
        public static bool IsValid(this IApplicationIdentifier appid)
        {
            return appid.DesignatedAreaCode < 1000;
        }
    }
}
