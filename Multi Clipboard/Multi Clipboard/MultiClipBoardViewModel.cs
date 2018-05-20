using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Multi_Clipboard
{


    public delegate void SetClipboardSize(int value);
    public delegate void PressedKeyOnApplication(string keyName, int virtualKey);
    public delegate void CloseShowSelectView();


    public class MultiClipBoardViewModel : Screen
    {

        ClipboardCore core;
        WindowManager _maneger;
        DispatcherTimer _showSelectTimer;

        public static SetClipboardSize SetClipboardSize;
        public static PressedKeyOnApplication CatchBindKey;
        public static CloseShowSelectView CloseWindow;

        private bool _isAssignHotkeyAvailable { get; set; }
        private Enums.KeyType _selectedKey { get; set; }



        /// <summary>
        /// App constructor
        /// </summary>
        public MultiClipBoardViewModel()
        {
            core = new ClipboardCore();
            _maneger = new WindowManager();
            _showSelectTimer = new DispatcherTimer();
            Clipboard.CurrentItem += SetCurrentlySelectedItem;
            CatchBindKey += AssignBindKey;
            CloseWindow += DoNothing;
            _showSelectTimer.Tick += ShowSelectTimerTick;

            this.DisplayName = "Multi Clipboard";          // Window display name
            FillComboboxes();                              // Setting up comboboxes    
        }


        #region Binded variable

        private bool _isCheckedPreviousClipboardHotkey;
        public bool isCheckedPreviousClipboardHotkey
        {
            get { return _isCheckedPreviousClipboardHotkey; }
            set
            {
                _isCheckedPreviousClipboardHotkey = value;
                if (value && isCheckedNextClipboardHotkey)
                {
                    isCheckedNextClipboardHotkey = false;
                }
                if (value)
                {
                    _isAssignHotkeyAvailable = true;
                    _selectedKey = Enums.KeyType.Previous;
                }
                else _isAssignHotkeyAvailable = false;
                NotifyOfPropertyChange("isCheckedPreviousClipboardHotkey");
            }
        }


        private bool _isCheckedNextClipboardHotkey;
        public bool isCheckedNextClipboardHotkey
        {
            get { return _isCheckedNextClipboardHotkey; }
            set
            {
                _isCheckedNextClipboardHotkey = value;
                if (value && isCheckedPreviousClipboardHotkey)
                {
                    isCheckedPreviousClipboardHotkey = false;
                }
                if (value)
                {
                    _isAssignHotkeyAvailable = true;
                    _selectedKey = Enums.KeyType.Next;
                }
                else _isAssignHotkeyAvailable = false;
                NotifyOfPropertyChange("isCheckedNextClipboardHotkey");
            }
        }


        private string _nextBindKeyName;
        public string nextBindKeyName
        {
            get { return _nextBindKeyName; }
            private set
            {
                _nextBindKeyName = value;
                NotifyOfPropertyChange("nextBindKeyName");
            }
        }


        private string _previousBindKeyName;
        public string previousBindKeyName
        {
            get { return _previousBindKeyName; }
            private set
            {
                _previousBindKeyName = value;
                NotifyOfPropertyChange("previousBindKeyName");
            }
        }


        private string _currentlySelectedItem;
        public string currentlySelectedItem
        {
            get { return _currentlySelectedItem; }
            private set
            {
                _currentlySelectedItem = value;
                NotifyOfPropertyChange("currentlySelectedItem");
            }
        }


        private string _previousHotkeyName;
        public string previousHotkeyName
        {
            get { return _previousHotkeyName; }
            private set
            {
                _previousHotkeyName = value;
                NotifyOfPropertyChange("previousHotkeyName");
            }
        }


        private List<Enums.Key> _previousHotkeySelector;
        public List<Enums.Key> previousHotkeySelector
        {
            get { return _previousHotkeySelector; }
            private set
            {
                _previousHotkeySelector = value;
                NotifyOfPropertyChange("previousHotkeySelector");
            }
        }

        private Enums.Key _SelectedpreviousHotkeySelector;
        public Enums.Key SelectedpreviousHotkeySelector
        {
            get { return _SelectedpreviousHotkeySelector; }
            set
            {
                _SelectedpreviousHotkeySelector = value;
                KeyCatcher.SetKey((int)value, Enums.KeyType.PreviousSpecial);
                NotifyOfPropertyChange("SelectedpreviousHotkeySelector");
            }
        }


        private string _nextHotkeyName;
        public string nextHotkeyName
        {
            get { return _nextHotkeyName; }
            private set
            {
                _nextHotkeyName = value;
                NotifyOfPropertyChange("nextHotkeyName");
            }
        }


        private List<int> _clipboardSize;
        public List<int> clipboardSize
        {
            get { return _clipboardSize; }
            private set
            {
                _clipboardSize = value;
                NotifyOfPropertyChange("clipboardSize");
            }
        }

        private int _selectedclipboardSize;
        public int SelectedclipboardSize
        {
            get { return _selectedclipboardSize; }
            set
            {
                _selectedclipboardSize = value;
                SetClipboardSize(value);
                NotifyOfPropertyChange("SelectedclipboardSize");
            }
        }


        private Enums.Key _SelectednextHotkeySelector;
        public Enums.Key SelectednextHotkeySelector
        {
            get { return _SelectednextHotkeySelector; }
            set
            {
                _SelectednextHotkeySelector = value;
                KeyCatcher.SetKey((int)value, Enums.KeyType.NextSpecial);
                NotifyOfPropertyChange("SelectednextHotkeySelector");
            }
        }

        private List<Enums.Key> _nextHotkeySelector;
        public List<Enums.Key> nextHotkeySelector
        {
            get { return _nextHotkeySelector; }
            private set
            {
                _nextHotkeySelector = value;
                NotifyOfPropertyChange("nextHotkeySelector");
            }
        }


        #endregion

        
        /// <summary>
        /// Avoid exception when ShowSelect Window is not enable
        /// </summary>
        private void DoNothing()
        {
        }

        /// <summary>
        /// Closing ShowSelect Window after 3 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSelectTimerTick(object sender, EventArgs e)
        {
            CloseWindow();
            _showSelectTimer.IsEnabled = false;
        }


        /// <summary>
        /// Assigning keyboard key to app action
        /// </summary>
        /// <param name="keyName"> Key name for display in view </param>
        /// <param name="virtualKey"> Virtual key code for watching press on it </param>
        private void AssignBindKey(string keyName, int virtualKey)
        {
            if(_isAssignHotkeyAvailable)
            {
                KeyCatcher.SetKey(virtualKey, _selectedKey);
                isCheckedNextClipboardHotkey = false;
                isCheckedPreviousClipboardHotkey = false;
                switch(_selectedKey)
                {
                    case Enums.KeyType.Next:
                        nextBindKeyName = keyName;
                        break;

                    case Enums.KeyType.Previous:
                        previousBindKeyName = keyName;
                        break;
                }
            }
        }


        /// <summary>
        /// Showing up new window under cursor with selected item name. Closing after 3 seconds
        /// </summary>
        /// <param name="value"> Item name or value if it is text </param>
        private void SetCurrentlySelectedItem(string value)
        {
            currentlySelectedItem = value.ToString();
            CloseWindow();
            _maneger.ShowWindow(new ShowSelectViewModel(value));
            _showSelectTimer.Interval = new TimeSpan(0, 0, 3);
            _showSelectTimer.IsEnabled = true;
        }
        

        /// <summary>
        /// Button click reaction
        /// Deleting currently seleced item from clipboard queue
        /// </summary>
        public void ClearCurrentSelection()
        {
            ClipboardCore.HotkeyPressReaction(Enums.Action.Delete);
        }


        /// <summary>
        /// Button click reaction.
        /// Clearing all items from clipboard queue
        /// </summary>
        public void ClearWholeClipboard()
        {
            ClipboardCore.HotkeyPressReaction(Enums.Action.ClearAll);
        }

        

        /// <summary>
        /// Filling every combobox with data. Selecting starting values
        /// </summary>        
        private void FillComboboxes()
        {            
            previousHotkeySelector = new List<Enums.Key>(new Enums.Key[] { Enums.Key.Ctrl, Enums.Key.Shift, Enums.Key.Alt });
            nextHotkeySelector = new List<Enums.Key>(new Enums.Key[] { Enums.Key.Ctrl, Enums.Key.Shift, Enums.Key.Alt });
            clipboardSize = new List<int>();

            for (int i = 5; i <= 20; i++)
            {
                clipboardSize.Add(i);
            }
            SelectedclipboardSize = 5;
            SelectedpreviousHotkeySelector = Enums.Key.Ctrl;
            SelectednextHotkeySelector = Enums.Key.Ctrl;
        }
       


    }
    


}
