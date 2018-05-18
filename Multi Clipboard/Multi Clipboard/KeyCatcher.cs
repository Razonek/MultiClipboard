using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Multi_Clipboard
{
    public static class KeyCatcher
    {


        private static int _previeusSpecialKey { get; set; }
        private static int _nextSpecialKey { get; set; }
        public static int _previeusKey { get; set; }
        public static int _nextKey { get; set; }


        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);



        /// <summary>
        /// Detecting if any combination of hotkeys is pressed.
        /// </summary>
        /// <returns></returns>
        public static Enums.Action DetectAnyHotkeyPress()
        {
            if (GetAsyncKeyState((int)Enums.Key.Ctrl) != 0)
            {
                if (GetAsyncKeyState((int)Enums.Key.C) != 0)
                    return Enums.Action.Copy;

                else if (GetAsyncKeyState((int)Enums.Key.X) != 0)
                    return Enums.Action.Cut;
            }

            if (GetAsyncKeyState(_previeusSpecialKey) != 0 && GetAsyncKeyState(_previeusKey) != 0)
                return Enums.Action.Previeus;

            if (GetAsyncKeyState(_nextSpecialKey) != 0 && GetAsyncKeyState(_nextKey) != 0)
                return Enums.Action.Next;

            else
                return Enums.Action.None;

        }

        /// <summary>
        /// Setting one of hotkeys with virtual key code
        /// </summary>
        /// <param name="value"> Key value</param>
        /// <param name="type"> Type of hotkey </param>
        public static void SetKey(int value, Enums.KeyType type)
        {
            switch (type)
            {
                case Enums.KeyType.Next:
                    _nextKey = value;
                    break;

                case Enums.KeyType.NextSpecial:
                    _nextSpecialKey = value;
                    break;

                case Enums.KeyType.Previeus:
                    _previeusKey = value;
                    break;

                case Enums.KeyType.PrevieusSpecial:
                    _previeusSpecialKey = value;
                    break;
            }
        }


    }
}
