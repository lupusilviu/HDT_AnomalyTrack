using Hearthstone_Deck_Tracker.Utility.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HDT_BGFightTracker
{
    public partial class StatisticsView : UserControl, IDisposable
    {
        private bool isDragging = false;
        private Point startPoint;
        private TranslateTransform transform = new TranslateTransform();

        public StatisticsView()
        {
            InitializeComponent();

            MovableControl.MouseDown += MovableControl_MouseDown;
            MovableControl.MouseMove += MovableControl_MouseMove;
            MovableControl.MouseUp += MovableControl_MouseUp;
            MovableControl.RenderTransform = transform;

            OverlayExtensions.SetIsOverlayHitTestVisible(MovableControl, true);
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


        public void Dispose()
        {
        }
    }
}
