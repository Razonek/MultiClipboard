using System;
using Caliburn.Micro;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows;

namespace Multi_Clipboard
{

    public delegate void CursorPosition(Point actualCursorPosition);


    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.X, point.Y);
        }
    }


    public class ShowSelectViewModel : Screen
    {

        public static CursorPosition ChangeWindowPosition;
        DispatcherTimer _refreshPosition;


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);


        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="selectName"> File name or text to show in window </param>
        public ShowSelectViewModel(string selectName)
        {
            this.selectName = selectName;
            MultiClipBoardViewModel.CloseWindow += CloseThisWindow;
            _refreshPosition = new DispatcherTimer();
            _refreshPosition.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _refreshPosition.Tick += RefreshPositionTick;
            _refreshPosition.Start();
        }

           

        private string _selectName;
        public string selectName
        {
            get { return _selectName; }
            private set
            {
                _selectName = value;
                NotifyOfPropertyChange("selectName");
            }
        }


        /// <summary>
        /// Close this Window
        /// </summary>
        private void CloseThisWindow()
        {
            _refreshPosition.Stop();
            this.TryClose();
        }
        

        /// <summary>
        /// Timer Tick for change window position if cursor move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshPositionTick(object sender, EventArgs e)
        {
            Point _currentCursorPosition = GetCursorPosition();
            ChangeWindowPosition(_currentCursorPosition);
            
        }

        /// <summary>
        /// Get current cursor position
        /// </summary>
        /// <returns> Cursor point </returns>
        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }



    }    



}
