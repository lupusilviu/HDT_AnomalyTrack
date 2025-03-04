using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Enums;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HDT_BGFightTracker
{
    internal class ViewModel
    {
        #region Members

        private bool _isInBattle;
        private bool? _currentRoundResult;
        private string _currentOppName;
        private int _currentRoundNumber;

        #endregion Members

        #region Properties
        public OpponentViewModel OpponentVM { get; private set; }

        public ICommand OnButtonPressedCommand { get; private set; }
        #endregion Properties



        public ViewModel()
        {
            OpponentVM = new OpponentViewModel();
            OnButtonPressedCommand = new RelayCommand(OnButtonPressedCommand_Execute);
        }

        #region OnButtonPressedCommand

        private void OnButtonPressedCommand_Execute(object parameter)
        {
            LogsHelper.WriteToFile("OnButtonPressedCommand_Execute");
            AttachToHDTEvents();
        }

        #endregion OnButtonPressedCommand

        #region Plugin Methods

        public void AttachToHDTEvents()
        {
            try
            {
                GameEvents.OnGameStart.Add(OnGameStart);
                GameEvents.OnTurnStart.Add(OnTurnStart);
                GameEvents.OnGameEnd.Add(OnGameEnded);
                GameEvents.OnOpponentGet.Add(OnOpponentGet);
                GameEvents.OnEntityWillTakeDamage.Add(EntityTakeDamage);
            }
            catch { }
        }

        public void OnGameStart()
        {
            try
            {
                LogsHelper.WriteToFile("OnGameStart");
                _isInBattle = false;
                _currentRoundResult = null;
                _currentOppName = "";
                _currentRoundNumber = 1;

                OpponentVM.SetCurrentOpponent(null);
                OpponentVM.SetIsVisible(true);
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR OnGameStart: " + ex.Message);
            }
        }

        public void OnTurnStart(ActivePlayer player)
        {
            try
            {
                LogsHelper.WriteToFile("OnTurnStart: " + player.ToString());
                _isInBattle = player == ActivePlayer.Opponent;
                if (_isInBattle == false)
                {
                    LogsHelper.WriteToFile("OnTurnStart - IsInBattle False");
                    if (string.IsNullOrEmpty(_currentOppName) == false)
                    {
                        OpponentDB.AddBattleInfo(_currentOppName, _currentRoundResult);
                        var opponent = OpponentDB.GetModel(_currentOppName);
                        OpponentVM.SetCurrentOpponent(opponent);

                        OpponentVM.AddRoundResults(_currentRoundNumber, _currentRoundResult);
                    }

                    _currentRoundResult = null;
                }
                else
                {
                    LogsHelper.WriteToFile("OnTurnStart - IsInBattle True");
                    _currentRoundNumber = Core.Game.GetTurnNumber();
                    //_lastRoundResult = new RoundResult();
                    //var current = Core.Game.Player.Hero;
                    //var armor = current.GetTag(GameTag.ARMOR);
                    //_lastRoundResult.HPBefore = current.Health + armor;
                    //_lastRoundResult.RoundNumber = Core.Game.GetTurnNumber();
                }
            }
            catch
            {

            }
        }

        public void OnGameEnded()
        {
            try
            {
                LogsHelper.WriteToFile("OnGameEnded");
                OpponentDB.AddBattleInfo(_currentOppName, _currentRoundResult);

                // Save at the end due to scenario where if you restart while playing, you get all the information again.
                OpponentDB.SaveDatabase();

                Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(10 * 1000);// Allow a refresh before hiding.
                    OpponentVM.SetIsVisible(false);
                });
            }
            catch { }
        }

        public void OnOpponentGet()
        {
            try
            {
                LogsHelper.WriteToFile("OnOpponentGet");
                if (_isInBattle == true)
                {
                    LogsHelper.WriteToFile("OnOpponentGet: IsInBattle True");
                    var opponent = Hearthstone_Deck_Tracker.Core.Game.OpponentEntity;

                    _currentOppName = opponent.Name;
                    if (opponent.CardId == "TB_BaconShop_HERO_KelThuzad")
                    {
                        // We are fighting a ghost
                        _currentOppName = "KelThuzad (Ghost)";
                    }

                    LogsHelper.WriteToFile("OnOpponentGet: Opponent: " + _currentOppName);
                    var opponentDB = OpponentDB.GetModel(_currentOppName);
                    OpponentVM.SetCurrentOpponent(opponentDB);
                }
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR OnOpponentGet: " + ex.Message);
            }
        }

        public void EntityTakeDamage(PredamageInfo info)
        {
            try
            {
                if (_isInBattle == false || info.Entity.IsHero == false)
                    return;

                LogsHelper.WriteToFile("EntityTakeDamage");

                var opponent = Hearthstone_Deck_Tracker.Core.Game.OpponentEntity;
                _currentOppName = opponent.Name;
                if (info.Entity.CardId == "TB_BaconShop_HERO_KelThuzad")
                {
                    // We are fighting a ghost
                    _currentOppName = "KelThuzad (Ghost)";
                }

                LogsHelper.WriteToFile("EntityTakeDamage: Opponent: " + _currentOppName);
                if (info.Entity.IsControlledBy(Core.Game.Player.Id))
                {
                    LogsHelper.WriteToFile("EntityTakeDamage. Round Lost");
                    _currentRoundResult = false;
                }
                else
                {
                    LogsHelper.WriteToFile("EntityTakeDamage. Round Won");
                    _currentRoundResult = true;
                }
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR EntityTakeDamage: " + ex.Message);
            }
        }

        #endregion Plugin Methods

        #region Private Methods

        private int _previousRound;
        private string _oppName;

        private void CheckForChanges(int round, int totalHealth, Hearthstone_Deck_Tracker.Hearthstone.Entities.Entity opponent)
        {
            if (opponent == null)
                return;

            if (_previousRound == round)
            {
                if (_oppName != opponent.Name)
                {
                    _oppName = opponent.Name;
                    LogsHelper.WriteToFile("BATTLE: STARTED " + _oppName);
                }
            }
            else
            {

                LogsHelper.WriteToFile("BATTLE: ENDED");
                _previousRound = round;
                _oppName = opponent.Name;
            }
        }

        #endregion Private Methods
    }
}
