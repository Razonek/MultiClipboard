using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Clipboard
{

    public delegate void LoadedintoMemory(object value);

     public class Clipboard
    {
        
        Queue<string> clipboard;                                    // memory
        private int clipboardSize { get; set; }                     // maximum item count in memory
        private int currentlySelectedItemIndex = 0;                 // currently selected item index in queue

        public static LoadedintoMemory CurrentItem;

        private string _currentlySelectedItem;                      // currently selected item to paste
        public string currentlySelectedItem
        {
            get { return _currentlySelectedItem; }
            private set
            {
                _currentlySelectedItem = value;
                if(value != null)
                {
                    System.Windows.Clipboard.SetText(value);
                    CurrentItem(currentlySelectedItem);
                }
                
            }
        }



        /// <summary>
        /// Class constructor
        /// </summary>
        public Clipboard()
        {
            clipboard = new Queue<string>();
            ClipboardCore.HotkeyPressReaction += UserAction;
            MultiClipBoardViewModel.SetClipboardSize += SetClipboardMaxSize;
        }


        /// <summary>
        /// App reaction for user hotkey press
        /// </summary>
        /// <param name="action"> Kind of user action </param>
        private void UserAction(Enums.Action action)
        {
            switch(action)
            {              

                case Enums.Action.Copy:
                    clipboard.Enqueue(System.Windows.Clipboard.GetText());
                    QueueSizeControl();
                    currentlySelectedItemIndex = clipboard.Count - 1;
                    currentlySelectedItem = clipboard.ElementAt(currentlySelectedItemIndex);
                    break;

                case Enums.Action.Next:
                    if(clipboard.Count > 1)
                    {
                        currentlySelectedItemIndex++;
                        if (currentlySelectedItemIndex > clipboard.Count - 1)
                            currentlySelectedItemIndex = 0;
                        currentlySelectedItem = clipboard.ElementAt(currentlySelectedItemIndex);
                    }                    
                    break;

                case Enums.Action.Previeus:
                    if(clipboard.Count > 1)
                    {
                        currentlySelectedItemIndex--;
                        if (currentlySelectedItemIndex < 0)
                            currentlySelectedItemIndex = clipboard.Count - 1;
                        currentlySelectedItem = clipboard.ElementAt(currentlySelectedItemIndex);
                    }                    
                    break;

                case Enums.Action.ClearAll:
                    clipboard.Clear();
                    currentlySelectedItem = null;
                    currentlySelectedItemIndex = 0;
                    break;

                case Enums.Action.Delete:
                    clipboard = new Queue<string>(clipboard.Where(x => x != currentlySelectedItem));
                    if (clipboard.Count > 0)
                        goto case Enums.Action.Previeus;
                    else
                        break;

                case Enums.Action.Cut:
                    goto case Enums.Action.Copy;
            }
        }

        /// <summary>
        /// Controling queue size for dont store more items than declared
        /// </summary>
        private void QueueSizeControl()
        {
            while (clipboard.Count > clipboardSize)
                clipboard.Dequeue();
        }

        /// <summary>
        /// Setting max clipboard size
        /// </summary>
        /// <param name="value"> Count of max memory size </param>
        private void SetClipboardMaxSize(int value)
        {
            clipboardSize = value;
            QueueSizeControl();
        }


    }


}
