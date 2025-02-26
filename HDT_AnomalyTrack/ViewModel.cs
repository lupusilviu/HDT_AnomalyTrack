using HearthDb.Enums;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Enums;
using System;
using System.IO;
using System.Windows.Documents;

namespace HDT_BGFightTracker
{
    internal class ViewModel
    {
        #region Members

        private string filePath = "C:\\temp\\logs\\myfile.txt";
        private RoundResult _lastRoundResult;
        private bool isInBattle;

        #endregion Members

        public OpponentViewModel OpponentVM { get; private set; }

        public ViewModel()
        {
            OpponentVM = new OpponentViewModel();
        }

        #region Plugin Methods

        internal void OnGameStart()
        {
            OpponentVM.SetIsVisible(true);
        }

        internal void OnTurnStart(ActivePlayer player)
        {
            try
            {
                isInBattle = player == ActivePlayer.Opponent;
                if (isInBattle == false)
                {
                    if (_lastRoundResult != null)
                    {
                        _lastRoundResult.Result = battleResult;
                        File.AppendAllLines(filePath, new string[]
                        {
                            String.Format( "LAST ROUND: {0}. {1} | R: {2} | Dmg: {3}",
                            _lastRoundResult.RoundNumber,
                            _lastRoundResult.OpponentName,
                            _lastRoundResult.Result,
                            _lastRoundResult.Damage)
                        });

                        bool? result = null;
                        if (_lastRoundResult.Result == "WON")
                            result = true;
                        else if (_lastRoundResult.Result == "LOST")
                            result = false;
                        OpponentDB.AddBattleInfo(_lastRoundResult.OpponentName, result);
                        var opponent = OpponentDB.GetModel(_lastRoundResult.OpponentName);
                        OpponentVM.SetCurrentOpponent(opponent);
                    }

                    File.AppendAllLines(filePath, new string[] { "TURN STARTED" });
                    battleResult = "DRAW";
                    _lastRoundResult = new RoundResult();
                }
                else
                {
                    UpdateOpponent();
                    _lastRoundResult = new RoundResult();

                    File.AppendAllLines(filePath, new string[] { "BATTLE STARTED" });
                    var current = Core.Game.Player.Hero;
                    var armor = current.GetTag(GameTag.ARMOR);
                    _lastRoundResult.HPBefore = current.Health + armor;
                    _lastRoundResult.RoundNumber = Core.Game.GetTurnNumber();
                }
            }
            catch
            {

            }
        }

        internal void OnGameEnded()
        {
            try
            {
                OpponentVM.SetIsVisible(false);
                if (_lastRoundResult != null)
                {
                    bool? result = null;
                    if (_lastRoundResult.Result == "WON")
                        result = true;
                    else if (_lastRoundResult.Result == "LOST")
                        result = false;

                    OpponentDB.AddBattleInfo(_lastRoundResult.OpponentName, result);
                }
            }
            catch { }
        }

        string battleResult = "DRAW";

        internal void OnOpponentGet()
        {
            if (isInBattle == true
                && _lastRoundResult != null
                && String.IsNullOrEmpty(_lastRoundResult.OpponentName))
            {
                UpdateOpponent();
            }
        }

        internal void EntityTakeDamage(PredamageInfo info)
        {
            try
            {
                if (!isInBattle || !info.Entity.IsHero) return;

                var opponent = Hearthstone_Deck_Tracker.Core.Game.OpponentEntity;
                _lastRoundResult.OpponentName = opponent.Name;
                File.AppendAllLines(filePath, new string[] { "FIGHTING GHOST ID:" + info.Entity.CardId+" ("+
                    opponent.IsPlayer+")"+" ("+opponent.IsInGraveyard+")"});

                if (info.Entity.IsControlledBy(Core.Game.Player.Id))
                {
                    //We dagamed
                    battleResult = "LOST";
                    _lastRoundResult.Damage = info.Value;
                }
                else
                {
                    battleResult = "WON";
                    _lastRoundResult.Damage = info.Value;
                }
            }
            catch { }
        }

        #endregion Plugin Methods

        #region Private Methods

        private void UpdateOpponent()
        {

        }

        #endregion Private Methods
    }
}
