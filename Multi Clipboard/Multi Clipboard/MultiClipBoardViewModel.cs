using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Multi_Clipboard
{

    public class MultiClipBoardViewModel : Screen
    {

        public MultiClipBoardViewModel()
        {
            this.DisplayName = "Multi Clipboard";          // Window display name
            FillComboboxes();                              // Setting up comboboxes

        }



        /// <summary>
        /// Button click reaction
        /// Deleting currently seleced item from clipboard queue
        /// </summary>
        public void ClearCurrentSelection()
        {

        }

        /// <summary>
        /// Button click reaction.
        /// Clearing all items from clipboard queue
        /// </summary>
        public void ClearWholeClipboard()
        {

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
