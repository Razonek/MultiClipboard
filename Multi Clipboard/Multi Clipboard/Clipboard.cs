using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Multi_Clipboard
{

    public delegate void LoadedintoMemory(string value);

     public class Clipboard
    {
        
        Queue<Item> clipboard;                                    // memory
        private int clipboardSize { get; set; }                     // maximum item count in memory
        private int currentlySelectedItemIndex = 0;                 // currently selected item index in queue

        public static LoadedintoMemory CurrentItem;

        private Item _currentlySelectedItem;                      // currently selected item to paste
        public Item currentlySelectedItem
        {
            get { return _currentlySelectedItem; }
            private set
            {
                _currentlySelectedItem = value;
                if(value != null)
                {
                    if (value.isText)
                    {
                        System.Windows.Clipboard.SetText(value.item[0]);
                        CurrentItem(value.item[0]);
                    }
                                            
                    else
                    {
                        System.Windows.Clipboard.SetFileDropList(value.item);
                        string name = Path.GetFileName(value.item[0]);
                        CurrentItem(name);
                    }                 
                    
                }
                
            }
        }


        /// <summary>
        /// Class constructor
        /// </summary>
        public Clipboard()
        {
            clipboard = new Queue<Item>();
            ClipboardCore.HotkeyPressReaction += UserAction;
            MultiClipBoardViewModel.SetClipboardSize += SetClipboardMaxSize;
        }


        private void CopyItem()
        {
            StringCollection _item = new StringCollection();
            bool _isText;

            if (System.Windows.Clipboard.ContainsText())
            {
                _item.Add(System.Windows.Clipboard.GetText());
                _isText = true;                                
            }               
             
            else
            {
                var item = System.Windows.Clipboard.GetFileDropList();
                foreach(string path in item)
                {
                    _item.Add(path);
                }
                _isText = false;                
            }

            clipboard.Enqueue(new Item(_item, _isText));
            QueueSizeControl();
            currentlySelectedItemIndex = clipboard.Count - 1;
            currentlySelectedItem = clipboard.ElementAt(currentlySelectedItemIndex);

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
                    CopyItem();
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
                    clipboard = new Queue<Item>(clipboard.Where(x => x != currentlySelectedItem));
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
