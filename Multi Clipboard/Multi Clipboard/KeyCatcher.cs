using System.Runtime.InteropServices;

namespace Multi_Clipboard
{
    public static class KeyCatcher
    {


        private static int _previousSpecialKey { get; set; }
        private static int _nextSpecialKey { get; set; }
        public static int _previousKey { get; set; }
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

            if (GetAsyncKeyState(_previousSpecialKey) != 0 && GetAsyncKeyState(_previousKey) != 0)
                return Enums.Action.Previous;

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

                case Enums.KeyType.Previous:
                    _previousKey = value;
                    break;

                case Enums.KeyType.PreviousSpecial:
                    _previousSpecialKey = value;
                    break;
            }
        }


    }
}
