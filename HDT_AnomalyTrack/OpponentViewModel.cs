using System.ComponentModel;

namespace HDT_BGFightTracker
{
    internal class OpponentViewModel : INotifyPropertyChanged
    {
        #region Members

        private OpponentStatisticsModel _opponent;
        private bool _isVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Members

        #region Properties

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
        }

        #endregion Constructor

        #region Public Methods

        public void SetIsVisible(bool value)
        {
            IsVisible = value;
            Opponent = null;
        }

        public void SetCurrentOpponent(OpponentStatisticsModel opponent)
        {
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
