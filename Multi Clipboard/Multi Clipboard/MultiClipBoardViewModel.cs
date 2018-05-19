using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Multi_Clipboard
{

    public delegate void SetClipboardSize(int value);
    public delegate void PressedKeyOnApplication(string keyName, int virtualKey);

    public class MultiClipBoardViewModel : Screen
    {

        ClipboardCore core;

        public static SetClipboardSize SetClipboardSize;
        public static PressedKeyOnApplication CatchBindKey;

        private bool _isAssignHotkeyAvailable { get; set; }
        private Enums.KeyType _selectedKey { get; set; }

        private bool _isCheckedPrevieusClipboardHotkey;
        public bool isCheckedPrevieusClipboardHotkey
        {
            get { return _isCheckedPrevieusClipboardHotkey; }
            set
            {
                _isCheckedPrevieusClipboardHotkey = value;
                if(value && isCheckedNextClipboardHotkey)
                {
                    isCheckedNextClipboardHotkey = false;
                }
                if (value)
                {
                    _isAssignHotkeyAvailable = true;
                    _selectedKey = Enums.KeyType.Previeus;
                }
                else _isAssignHotkeyAvailable = false;
                NotifyOfPropertyChange("isCheckedPrevieusClipboardHotkey");
            }
        }

        private bool _isCheckedNextClipboardHotkey;
        public bool isCheckedNextClipboardHotkey
        {
            get { return _isCheckedNextClipboardHotkey; }
            set
            {
                _isCheckedNextClipboardHotkey = value;
                if(value && isCheckedPrevieusClipboardHotkey)
                {
                    isCheckedPrevieusClipboardHotkey = false;
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

        private string _previeusBindKeyName;
        public string previeusBindKeyName
        {
            get { return _previeusBindKeyName; }
            private set
            {
                _previeusBindKeyName = value;
                NotifyOfPropertyChange("previeusBindKeyName");
            }
        }




        public MultiClipBoardViewModel()
        {
            this.DisplayName = "Multi Clipboard";          // Window display name
            core = new ClipboardCore();
            FillComboboxes();                              // Setting up comboboxes            
            Clipboard.CurrentItem += SetCurrentlySelectedItem;
            CatchBindKey += AssignBindKey;     
            
        }



        private void AssignBindKey(string keyName, int virtualKey)
        {
            if(_isAssignHotkeyAvailable)
            {
                KeyCatcher.SetKey(virtualKey, _selectedKey);
                isCheckedNextClipboardHotkey = false;
                isCheckedPrevieusClipboardHotkey = false;
                switch(_selectedKey)
                {
                    case Enums.KeyType.Next:
                        nextBindKeyName = keyName;
                        break;

                    case Enums.KeyType.Previeus:
                        previeusBindKeyName = keyName;
                        break;
                }
            }
        }


        private void SetCurrentlySelectedItem(object value)
        {
            currentlySelectedItem = value.ToString();
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


        private string _previeusHotkeyName;
        public string previeusHotkeyName
        {
            get { return _previeusHotkeyName; }
            private set
            {
                _previeusHotkeyName = value;
                NotifyOfPropertyChange("previeusHotkeyName");
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


        /// <summary>
        /// Max clipboard items count - View
        /// </summary>
        #region
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
        #endregion 

        /// <summary>
        /// Selector for "previeus" hotkey - View
        /// </summary>
        #region
        private List<Enums.Key> _previeusHotkeySelector;
        public List<Enums.Key> previeusHotkeySelector
        {
            get { return _previeusHotkeySelector; }
            private set
            {
                _previeusHotkeySelector = value;
                NotifyOfPropertyChange("previeusHotkeySelector");
            }
        }

        private Enums.Key _SelectedprevieusHotkeySelector;
        public Enums.Key SelectedprevieusHotkeySelector
        {
            get { return _SelectedprevieusHotkeySelector; }
            set
            {
                _SelectedprevieusHotkeySelector = value;
                KeyCatcher.SetKey((int)value, Enums.KeyType.PrevieusSpecial);
                NotifyOfPropertyChange("SelectedprevieusHotkeySelector");
            }
        }
        #endregion

        /// <summary>
        /// Selector for "next" hotkey - View
        /// </summary>
        #region
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
        /// Filling every combobox with data. Selecting starting values
        /// </summary>
        #region FillComboboxes
        private void FillComboboxes()
        {            
            previeusHotkeySelector = new List<Enums.Key>(new Enums.Key[] { Enums.Key.Ctrl, Enums.Key.Shift, Enums.Key.Alt });
            nextHotkeySelector = new List<Enums.Key>(new Enums.Key[] { Enums.Key.Ctrl, Enums.Key.Shift, Enums.Key.Alt });
            clipboardSize = new List<int>();

            for (int i = 5; i <= 20; i++)
            {
                clipboardSize.Add(i);
            }
            SelectedclipboardSize = 5;
            SelectedprevieusHotkeySelector = Enums.Key.Ctrl;
            SelectednextHotkeySelector = Enums.Key.Ctrl;
        }
        #endregion
        





    }





}
