namespace SportBattles.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "SportBattles";

        public const string AdministratorRoleName = "Administrator";

        public const byte LiveScoreAPITimeZoneCorrection = 7;
        public const byte LiveScoreAPIDaysAhead = 7;
        public static readonly string[] LiveScoreApiCategories = new string[5] { "Soccer", "Cricket", "Basketball", "Tennis", "Hockey" };
    }
}
