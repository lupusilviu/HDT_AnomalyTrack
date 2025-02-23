using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Extensions;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HDT_AnomalyTrack
{
    /// <summary>
    /// Interaction logic for AnomalyPanel.xaml
    /// </summary>
    public partial class AnomalyPanel : UserControl, IDisposable
    {
        private bool isDragging = false;
        private Point startPoint;
        private TranslateTransform transform = new TranslateTransform();

        public AnomalyPanel()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;

            MovableControl.MouseDown += MovableControl_MouseDown;
            MovableControl.MouseMove += MovableControl_MouseMove;
            MovableControl.MouseUp += MovableControl_MouseUp;
            MovableControl.RenderTransform = transform;
        }

        private void MovableControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                startPoint = e.GetPosition(this);
                MovableControl.CaptureMouse();
            }
        }

        private void MovableControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPoint = e.GetPosition(this);
                double offsetX = currentPoint.X - startPoint.X;
                double offsetY = currentPoint.Y - startPoint.Y;

                transform.X += offsetX;
                transform.Y += offsetY;

                startPoint = currentPoint; // Update the starting point
            }
        }

        private void MovableControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            MovableControl.ReleaseMouseCapture();
        }

        public void OnDisplayAnomaly(int id)
        {
            try
            {
                Visibility = Visibility.Visible;

                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = GetUriBasedOnId(id);
                logo.EndInit();
                MainImg.Source = logo;
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
        }

        private Uri GetUriBasedOnId(int id)
        {
            try
            {
                string guid = "";
                switch (id)
                {
                    case 119093:
                        {
                            guid = "BGDUO_Anomaly_006";
                            break;
                        }
                    case 102093:
                        {
                            guid = "BG27_Anomaly_755";
                            break;
                        }
                    case 119142:
                        {
                            guid = "BGDUO_Anomaly_005";
                            break;
                        }
                    case 102982:
                        {
                            guid = "BG27_Anomaly_803";
                            break;
                        }
                    case 106500:
                        {
                            guid = "BG27_Anomaly_562";
                            break;
                        }
                    case 107226:
                        {
                            guid = "BG27_Anomaly_580";
                            break;
                        }
                    case 102076:
                        {
                            guid = "BG27_Anomaly_302";
                            break;
                        }
                    case 103541:
                        {
                            guid = "BG27_Anomaly_810";
                            break;
                        }
                    case 118697:
                        {
                            guid = "BG31_Anomaly_102";
                            break;
                        }
                    case 119094:
                        {
                            guid = "BG31_Anomaly_123";
                            break;
                        }
                    case 103113:
                        {
                            guid = "BG27_Anomaly_822";
                            break;
                        }
                    case 102046:
                        {
                            guid = "BG27_Anomaly_711";
                            break;
                        }
                    case 106633:
                        {
                            guid = "BG27_Anomaly_575";
                            break;
                        }
                    case 118734:
                        {
                            guid = "BG31_Anomaly_111";
                            break;
                        }
                    case 118676:
                        {
                            guid = "BG31_Anomaly_117";
                            break;
                        }
                    case 118670:
                        {
                            guid = "BG31_Anomaly_114";
                            break;
                        }
                    case 102075:
                        {
                            guid = "BG27_Anomaly_301";
                            break;
                        }
                    case 102455:
                        {
                            guid = "BG27_Anomaly_715";
                            break;
                        }
                    case 119044:
                        {
                            guid = "BG31_Anomaly_124";
                            break;
                        }
                    case 119157:
                        {
                            guid = "BGDUO_Anomaly_003";
                            break;
                        }
                    case 102005:
                        {
                            guid = "BG27_Anomaly_900";
                            break;
                        }
                    case 102085:
                        {
                            guid = "BG27_Anomaly_303";
                            break;
                        }
                    case 118700:
                        {
                            guid = "BG31_Anomaly_112";
                            break;
                        }
                    case 119749:
                        {
                            guid = "BG31_Anomaly_127";
                            break;
                        }
                    case 118680:
                        {
                            guid = "BG31_Anomaly_101";
                            break;
                        }
                    case 118673:
                        {
                            guid = "BG31_Anomaly_116";
                            break;
                        }
                    case 118674:
                        {
                            guid = "BG31_Anomaly_115";
                            break;
                        }
                    case 118669:
                        {
                            guid = "BG31_Anomaly_106";
                            break;
                        }
                    case 102989:
                        {
                            guid = "BG27_Anomaly_805";
                            break;
                        }
                    case 101992:
                        {
                            guid = "BG27_Anomaly_005";
                            break;
                        }
                    case 101954:
                        {
                            guid = "BG27_Anomaly_000";
                            break;
                        }
                    case 118671:
                        {
                            guid = "BG31_Anomaly_109";
                            break;
                        }
                    case 102748:
                        {
                            guid = "BG27_Anomaly_720";
                            break;
                        }
                    case 106631:
                        {
                            guid = "BG27_Anomaly_577";
                            break;
                        }
                    case 102457:
                        {
                            guid = "BG27_Anomaly_718";
                            break;
                        }
                    case 102083:
                        {
                            guid = "BG27_Anomaly_750";
                            break;
                        }
                    case 102092:
                        {
                            guid = "BG27_Anomaly_754";
                            break;
                        }
                    case 102084:
                        {
                            guid = "BG27_Anomaly_751";
                            break;
                        }
                    case 119748:
                        {
                            guid = "BG31_Anomaly_126";
                            break;
                        }
                    case 119143:
                        {
                            guid = "BGDUO_Anomaly_007";
                            break;
                        }
                    case 101986:
                        {
                            guid = "BG27_Anomaly_002";
                            break;
                        }
                    case 118733:
                        {
                            guid = "BG31_Anomaly_104";
                            break;
                        }
                    case 119092:
                        {
                            guid = "BG31_Anomaly_120";
                            break;
                        }
                    case 103078:
                        {
                            guid = "BG27_Anomaly_504";
                            break;
                        }
                    case 118688:
                        {
                            guid = "BG31_Anomaly_105";
                            break;
                        }
                    case 102875:
                        {
                            guid = "BG27_Anomaly_723";
                            break;
                        }
                    case 102991:
                        {
                            guid = "BG27_Anomaly_103";
                            break;
                        }
                    case 102971:
                        {
                            guid = "BG27_Anomaly_501";
                            break;
                        }
                    case 102090:
                        {
                            guid = "BG27_Anomaly_801";
                            break;
                        }
                    case 102976:
                        {
                            guid = "BG27_Anomaly_503";
                            break;
                        }
                    case 109379:
                        {
                            guid = "BG27_Anomaly_573";
                            break;
                        }
                    case 106510:
                        {
                            guid = "BG27_Anomaly_572";
                            break;
                        }
                    case 106508:
                        {
                            guid = "BG27_Anomaly_571";
                            break;
                        }
                    case 106507:
                        {
                            guid = "BG27_Anomaly_570";
                            break;
                        }
                    case 106470:
                        {
                            guid = "BG27_Anomaly_559";
                            break;
                        }
                    case 102792:
                        {
                            guid = "BG27_Anomaly_721";
                            break;
                        }
                    case 102459:
                        {
                            guid = "BG27_Anomaly_716";
                            break;
                        }
                }

                return new Uri("pack://application:,,,/HDT_AnomalyTrack;component/Images/" + guid + ".png");
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
