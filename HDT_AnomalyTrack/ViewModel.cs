using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Enums;
using System.Threading.Tasks;

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

        public OpponentViewModel OpponentVM { get; private set; }

        public ViewModel()
        {
            OpponentVM = new OpponentViewModel();
        }

        #region Plugin Methods

        internal void OnGameStart()
        {
            OpponentVM.SetCurrentOpponent(null);
            OpponentVM.SetIsVisible(true);
        }

        internal void OnTurnStart(ActivePlayer player)
        {
            try
            {
                _isInBattle = player == ActivePlayer.Opponent;
                if (_isInBattle == false)
                {
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
                    //_lastRoundResult = new RoundResult();
                    //var current = Core.Game.Player.Hero;
                    //var armor = current.GetTag(GameTag.ARMOR);
                    //_lastRoundResult.HPBefore = current.Health + armor;
                    //_lastRoundResult.RoundNumber = Core.Game.GetTurnNumber();
                    _currentRoundNumber = Core.Game.GetTurnNumber();
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

        internal void OnOpponentGet()
        {

            try
            {
                if (_isInBattle == true)
                {
                    var opponent = Hearthstone_Deck_Tracker.Core.Game.OpponentEntity;

                    _currentOppName = opponent.Name;
                    if (opponent.CardId == "TB_BaconShop_HERO_KelThuzad")
                    {
                        // We are fighting a ghost
                        _currentOppName = "KelThuzad (Ghost)";
                    }

                    var opponentDB = OpponentDB.GetModel(_currentOppName);
                    OpponentVM.SetCurrentOpponent(opponentDB);
                }
            }
            catch { }
        }

        internal void EntityTakeDamage(PredamageInfo info)
        {
            try
            {
                if (!_isInBattle || !info.Entity.IsHero) return;

                var opponent = Hearthstone_Deck_Tracker.Core.Game.OpponentEntity;
                _currentOppName = opponent.Name;
                if (info.Entity.CardId == "TB_BaconShop_HERO_KelThuzad")
                {
                    // We are fighting a ghost
                    _currentOppName = "KelThuzad (Ghost)";
                }

                if (info.Entity.IsControlledBy(Core.Game.Player.Id))
                {
                    _currentRoundResult = false;
                }
                else
                {
                    _currentRoundResult = true;
                }
            }
            catch { }
        }

        #endregion Plugin Methods
    }
}
