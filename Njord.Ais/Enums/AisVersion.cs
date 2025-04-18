namespace Njord.Ais.Enums
{
    /// <summary>
    /// Represents the version of the AIS (Automatic Identification System) standard.
    /// </summary>
    public enum AisVersion : byte
    {
        /// <summary>
        /// Compliant with ITU-R M.1371-1 recommendation.
        /// </summary>
        CompliantWithRecommendationITU_RM1371_1 = 0,

        /// <summary>
        /// Compliant with ITU-R M.1371-3 recommendation.
        /// </summary>
        CompliantWithRecommendationITU_RM1371_3 = 1,

        /// <summary>
        /// Compliant with ITU-R M.1371-5 recommendation.
        /// </summary>
        CompliantWithRecommendationITU_RM1371_5 = 2,

        /// <summary>
        /// Compliant with future editions of the ITU-R M.1371 recommendation.
        /// </summary>
        CompliantWithFutureEditions = 3
    }
}
