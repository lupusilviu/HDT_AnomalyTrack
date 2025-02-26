namespace HDT_BGFightTracker
{
    internal class OpponentStatisticsModel
    {
        public string OpponentName { get; set; }

        public int TotalBattles { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

        public long LastFightBinary { get; set; }

    }
}
