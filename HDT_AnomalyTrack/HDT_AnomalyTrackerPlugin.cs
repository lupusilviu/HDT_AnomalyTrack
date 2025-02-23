using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using HearthMirror;
using System;
using System.IO;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Utility.MVVM;

namespace HDT_AnomalyTrack
{
    //https://github.com/IBM5100o/HDT_BGrank/blob/main/HDT_BGrank/BGrankPlugin.cs
    public class HDT_AnomalyTrackerPlugin : IPlugin
    {
        public string Name => "HDT_AnomalyTrack";

        public string Description => "Displays the current used anomaly";

        public string ButtonText => "Anomaly Track";

        public string Author => "RDUChatter";

        public Version Version => new Version(1, 0, 1);

        public MenuItem MenuItem { get; private set; }

        private ViewModel _viewModel;
        private AnomalyPanel _panel;

        public HDT_AnomalyTrackerPlugin()
        {
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
                File.AppendAllText("C:\\TEst\\Logs\\HSLogs.txt", "CreateVisual" + Environment.NewLine);
                _panel = new AnomalyPanel();
                _panel.DataContext = _viewModel;
                _viewModel.SetPanel(_panel);
                _viewModel.TryLoadAlreadyStarted();
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

                            Core.OverlayCanvas.Children.Add(_panel);
                        }
                        catch (Exception ex)
                        {
                           
                        }
                    };

                    MenuItem.Unchecked += (sender, args) =>
                    {
                        Core.OverlayCanvas.Children.Remove(_panel);
                        // _panel = null;
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
