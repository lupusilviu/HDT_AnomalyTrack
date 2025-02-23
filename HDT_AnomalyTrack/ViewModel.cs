using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HDT_AnomalyTrack
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private AnomalyPanel _panel;
        private Uri _anomalyUri;

        public ViewModel()
        {

        }

        public void SetPanel(AnomalyPanel panel)
        {
            _panel = panel;
        }

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Plugin Methods

        internal void OnGameStart()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(3 * 1000);
                        if (_panel == null)
                        {
                            continue;
                        }

                        Entity gameEntity = Core.Game.GameEntity;
                        if (gameEntity == null)
                            return;

                        var id = BattlegroundsUtils.GetBattlegroundsAnomalyDbfId(gameEntity);
                        if (id.HasValue)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    _panel.OnDisplayAnomaly(id.Value);
                                }
                                catch { }
                            });
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                }

            });
        }

        public void TryLoadAlreadyStarted()
        {
            try
            {
                Entity gameEntity = Core.Game.GameEntity;
                if (gameEntity == null)
                    return;

                var id = BattlegroundsUtils.GetBattlegroundsAnomalyDbfId(gameEntity);

                if (id.HasValue)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            _panel.OnDisplayAnomaly(id.Value);
                        }
                        catch { }
                    });
                }
            }
            catch { }
        }

        internal void OnTurnStart(ActivePlayer player)
        {

        }

        internal void OnGameEnded()
        {
           if(_panel!=null)
            {
                _panel.Visibility = Visibility.Hidden;
            }
        }



        #endregion Plugin Methods
    }
}
