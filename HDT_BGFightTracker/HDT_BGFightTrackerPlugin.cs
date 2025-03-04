using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Windows.Controls;

namespace HDT_BGFightTracker
{
    public class HDT_BGFightTrackererPlugin : IPlugin
    {
        #region Members

        private ViewModel _viewModel;
        private StatisticsView _panel;

        #endregion Members

        #region IPlugin Properties

        public string Name => "HDT_BGFightTracker";

        public string Description => "Displays the 1v1 statistics";

        public string ButtonText => "BG Fight Tracker";

        public string Author => "RDUChatter";

        public Version Version => new Version(1, 0, 0);

        public MenuItem MenuItem { get; private set; }

        #endregion IPlugin Properties

        #region Constructor

        public HDT_BGFightTrackererPlugin()
        {
            Initialize();
        }

        #endregion Constructor

        #region IPlugin Methods

        public void OnButtonPress()
        {

        }

        public void OnLoad()
        {
            LogsHelper.WriteToFile("OnLoad");
            CreateVisual();
            MenuItem.IsChecked = true;
        }

        public void OnUnload()
        {
            LogsHelper.WriteToFile("OnUnload");
            MenuItem.IsChecked = false;
        }

        public void OnUpdate()
        {
            //This is called very often.
        }

        #endregion IPlugin Methods

        #region Private Methods

        private void CreateVisual()
        {
            try
            {
                if (MenuItem == null)
                {
                    if (_viewModel == null)
                        Initialize();

                    LogsHelper.WriteToFile("CreateVisual: Attaching to events");
                    MenuItem = new MenuItem()
                    {
                        Header = ButtonText
                    };
                    MenuItem.IsCheckable = true;

                    MenuItem.Checked += (sender, args) =>
                    {
                        try
                        {
                            LogsHelper.WriteToFile("CreateVisual: Attaching");
                            _viewModel.AttachToHDTEvents();

                            Core.OverlayCanvas.Children.Add(_panel);
                            LogsHelper.WriteToFile("CreateVisual: Panel Added");
                        }
                        catch (Exception ex)
                        {
                            LogsHelper.WriteToFile("ERR CreateVisual: " + ex.Message);
                        }
                    };

                    MenuItem.Unchecked += (sender, args) =>
                    {
                        Core.OverlayCanvas.Children.Remove(_panel);
                    };
                }
                else
                {
                    LogsHelper.WriteToFile("CreateVisual: Already Attached to events");
                }
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR CreateVisual " + ex.Message);
            }
        }

        private void Initialize()
        {
            try
            {
                LogsHelper.WriteToFile("CreateVisual: Init");

                OpponentDB.LoadDatabase();
                _viewModel = new ViewModel();
                _panel = new StatisticsView();
                _panel.DataContext = _viewModel;
            }
            catch (Exception ex)
            {
                LogsHelper.WriteToFile("ERR CreateVisual: " + ex.Message);
            }
        }

        #endregion Private Methods
    }
}
