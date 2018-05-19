using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Multi_Clipboard
{
    /// <summary>
    /// Interaction logic for MultiClipBoardView.xaml
    /// </summary>
    public partial class MultiClipBoardView : Window
    {
        public MultiClipBoardView()
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(KeyPress);
        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            int virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
            string pressedKey = e.Key.ToString();
            MultiClipBoardViewModel.CatchBindKey(pressedKey, virtualKey);
        }



    }

}
