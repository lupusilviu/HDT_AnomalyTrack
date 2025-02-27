using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.IO;
using System.Windows.Controls;

namespace HDT_BGFightTracker
{
    public class HDT_BGFightTrackererPlugin : IPlugin
    {
        #region IPlugin Properties

        public string Name => "HDT_BGFightTracker";

        public string Description => "Displays the 1v1 statistics";

        public string ButtonText => "BG Fight Tracker";

        public string Author => "RDUChatter";

        public Version Version => new Version(1, 0, 0);

        public MenuItem MenuItem { get; private set; }

        #endregion IPlugin Properties

        private ViewModel _viewModel;
        private StatisticsView _panel;

        public HDT_BGFightTrackererPlugin()
        {
            OpponentDB.LoadDatabase();
            _viewModel = new ViewModel();
        }

        public void OnButtonPress()
        {

        }

        public void OnLoad()
        {
            //STARTED
            CreateVisual();
            MenuItem.IsChecked = true;
        }

        public void OnUnload()
        {
            //ENDED
            MenuItem.IsChecked = false;
        }

        public void OnUpdate()
        {

        }

        private void CreateVisual()
        {
            try
            {
                _panel = new StatisticsView();
                _panel.DataContext = _viewModel;

                if (MenuItem == null)
                {
                    MenuItem = new MenuItem()
                    {
                        Header = ButtonText
                    };
                    MenuItem.IsCheckable = true;

                    MenuItem.Checked += async (sender, args) =>
                    {
                        try
                        {
                            GameEvents.OnGameStart.Add(_viewModel.OnGameStart);
                            GameEvents.OnTurnStart.Add(_viewModel.OnTurnStart);
                            GameEvents.OnGameEnd.Add(_viewModel.OnGameEnded);
                            GameEvents.OnOpponentGet.Add(_viewModel.OnOpponentGet);
                            GameEvents.OnEntityWillTakeDamage.Add(_viewModel.EntityTakeDamage);

                            Core.OverlayCanvas.Children.Add(_panel);
                        }
                        catch (Exception ex)
                        {

                        }
                    };

                    MenuItem.Unchecked += (sender, args) =>
                    {
                        Core.OverlayCanvas.Children.Remove(_panel);
                    };
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("C:\\TEst\\Logs\\HSLogs.txt", "ERR1:" + ex.Message + Environment.NewLine);
            }
        }
    }
}
