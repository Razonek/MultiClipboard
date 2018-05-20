using System.Windows;


namespace Multi_Clipboard
{
    /// <summary>
    /// Interaction logic for ShowSelectView.xaml
    /// </summary>
    public partial class ShowSelectView : Window
    {
        public ShowSelectView()
        {
            InitializeComponent();
            ShowSelectViewModel.ChangeWindowPosition += SetPosition;
        }

        private void SetPosition(Point cursorPoint)
        {
            this.Top = cursorPoint.Y + 10;
            this.Left = cursorPoint.X + 10;
        }

    }
}
