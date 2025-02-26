using System;
using System.Collections.Generic;

namespace HDT_BGFightTracker
{
    internal class OpponentDB
    {
        private static Dictionary<string, OpponentStatisticsModel> _database;

        #region Public Methods

        public static OpponentStatisticsModel GetModel(string opponent)
        {
            OpponentStatisticsModel result = new OpponentStatisticsModel();
            try
            {
                if (_database.TryGetValue(opponent, out result))
                    return result;

                result = null;
            }
            catch { }

            return result;
        }

        public static void AddBattleInfo(string opponent, bool? result)
        {
            try
            {
                OpponentStatisticsModel opp = new OpponentStatisticsModel();
                if (_database.TryGetValue(opponent, out opp) == false)
                {
                    opp = new OpponentStatisticsModel()
                    {
                        OpponentName = opponent,
                        Draws = 0,
                        Wins = 0,
                        Losses = 0
                    };

                    _database.Add(opponent, opp);
                }

                opp.LastFightBinary = DateTime.Now.ToBinary();
                if (result.HasValue == false)
                {
                    opp.Draws += 1;
                }
                else if (result.Value == true)
                {
                    opp.Wins += 1;
                }
                else
                {
                    opp.Losses += 1;
                }
            }
            catch { }
        }

        public static void LoadDatabase()
        {
            try
            {
                //TODO: Load from local file
            }
            catch { }
        }

        public static void SaveDatabase()
        {
            try
            {
                //TODO: Save to local file
            }
            catch { }
        }

        #endregion Public Methods
    }
}
