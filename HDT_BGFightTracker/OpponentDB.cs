using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HDT_BGFightTracker
{
    internal class OpponentDB
    {
        #region Members

        private static Dictionary<string, OpponentStatisticsModel> _database;
        private static readonly string _localFileName = "HDT_BGFightTracker_DB.txt";
        #endregion Members

        #region Public Methods

        public static OpponentStatisticsModel GetModel(string opponent)
        {
            OpponentStatisticsModel result = new OpponentStatisticsModel();
            try
            {
                if (_database.TryGetValue(opponent, out result))
                    return result;

                result = new OpponentStatisticsModel()
                {
                    OpponentName = opponent,
                    Draws = 0,
                    Wins = 0,
                    Losses = 0,
                    TotalBattles = 0,
                    LastFightBinary = 0
                };
            }
            catch(Exception ex)
            {
                LogsHelper.WriteToFile("ERR DB GetModel: " + ex.Message);
            }

            return result;
        }

        public static void AddBattleInfo(string opponent, bool? result)
        {
            try
            {
                LogsHelper.WriteToFile("DB: AddBattleInfo " + opponent);

                OpponentStatisticsModel opp = new OpponentStatisticsModel();
                if (_database.TryGetValue(opponent, out opp) == false)
                {
                    opp = new OpponentStatisticsModel()
                    {
                        OpponentName = opponent,
                        Draws = 0,
                        Wins = 0,
                        Losses = 0,
                        TotalBattles = 0
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
                opp.TotalBattles += 1;
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR DB AddBattleInfo: " + ex.Message);
            }
        }

        public static void LoadDatabase()
        {
            try
            {
                LogsHelper.WriteToFile("DB: LoadDatabase");
                string tempPath = Path.GetTempPath();
                string filePath = Path.Combine(tempPath, _localFileName);
                if (File.Exists(filePath) == true)
                {
                    string content = File.ReadAllText(filePath);
                    _database = JsonConvert.DeserializeObject<Dictionary<string, OpponentStatisticsModel>>(content);
                }
                else
                {
                    LogsHelper.WriteToFile("DB: LoadDatabase. No previous data found");
                    _database = new Dictionary<string, OpponentStatisticsModel>();
                    SaveDatabase();
                }
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR DB LoadDatabase: " + ex.Message);
            }
        }

        public static void SaveDatabase()
        {
            try
            {
                LogsHelper.WriteToFile("DB: SaveDatabase");
                string serializedContent = JsonConvert.SerializeObject(_database);

                //On version update, the folder changes.
                string tempPath = Path.GetTempPath();
                string filePath = Path.Combine(tempPath, _localFileName);
                File.WriteAllText(filePath, serializedContent);
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR DB SaveDatabase: " + ex.Message);
            }
        }

        #endregion Public Methods
    }
}
