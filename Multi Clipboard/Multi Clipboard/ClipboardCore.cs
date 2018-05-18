using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Multi_Clipboard
{


    public delegate void HotkeyPressReaction(Enums.Action action);

    public class ClipboardCore
    {

        DispatcherTimer _coreClock;
        public static HotkeyPressReaction HotkeyPressReaction;
        Clipboard clipboard;
        private Enums.Action _action { get; set; }

        public ClipboardCore()
        {
            clipboard = new Clipboard();
            _coreClock = new DispatcherTimer();
            _coreClock.Interval = new TimeSpan(0, 0, 0, 0, 20);
            _coreClock.Tick += CoreClockTick;
            _coreClock.Start();
        }


        private void CoreClockTick(object sender, EventArgs e)
        {
            _action = KeyCatcher.DetectAnyHotkeyPress();

            if(_action != Enums.Action.None)
            {
                HotkeyPressReaction(_action);
                _coreClock.IsEnabled = false;
                Thread.Sleep(320);
                _coreClock.IsEnabled = true;
            }
            

        }



    }
}
