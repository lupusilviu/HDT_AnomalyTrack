using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;

namespace HDT_BGFightTracker
{
    internal class OpponentViewModel : INotifyPropertyChanged
    {
        #region Members

        private OpponentStatisticsModel _opponent;
        private bool _isVisible;
        private object _lockBR;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Members

        #region Properties

        public ObservableCollection<RoundResult> BattleResults { get; private set; }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            private set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        public OpponentStatisticsModel Opponent
        {
            get
            {
                return _opponent;
            }
            private set
            {
                _opponent = value;
                RaisePropertyChanged("Opponent");
            }
        }



        #endregion Properties

        #region Constructor

        public OpponentViewModel()
        {
            IsVisible = false;
            Opponent = null;
            _lockBR = new object();
            BattleResults = new ObservableCollection<RoundResult>();
            BindingOperations.EnableCollectionSynchronization(BattleResults, _lockBR);
        }

        #endregion Constructor

        #region Public Methods

        public void AddRoundResults(int nb, bool? result)
        {
            RoundResult newRound = new RoundResult()
            {
                RoundNumber = nb
            };

            if (result.HasValue == false)
            {
                newRound.Result = 0;
            }
            else if (result.Value == true)
            {
                newRound.Result = 1;
            }
            else
            {
                newRound.Result = -1;
            }

            BattleResults.Add(newRound);
        }

        public void SetIsVisible(bool value)
        {
            try
            {
                IsVisible = value;
                Opponent = null;
                while (BattleResults.Count > 0)
                    BattleResults.RemoveAt(0);
            }
            catch { }
        }

        public void SetCurrentOpponent(OpponentStatisticsModel opponent)
        {
            // File.AppendAllLines(@"C:\test\logs\myfile.txt",
            //    new string[] { "Opponent Update: " + (opponent != null) });
            Opponent = opponent;
        }

        #endregion Public Methods

        #region Methods

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion Methods
    }
}
