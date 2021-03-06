﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Windows;

namespace Multi_Clipboard
{

    public delegate void LoadedintoMemory(string value);

    public class Clipboard
    {

        private Queue<Item> clipboard;                                    // memory
        private int clipboardSize { get; set; }                     // maximum item count in memory
        private int currentlySelectedItemIndex = 0;                 // currently selected item index in queue
        private Enums.Action userAction{get;set;}

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


        /// <summary>
        /// Copy item or multiple items and set it as one object. If file is cutting then make copy in environment directory
        /// </summary>
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
                switch(userAction)
                {
                    case Enums.Action.Copy:
                        foreach (string path in System.Windows.Clipboard.GetFileDropList())
                        {
                            _item.Add(path);
                        }
                        break;

                    case Enums.Action.Cut:
                        foreach (string path in System.Windows.Clipboard.GetFileDropList())
                        {
                            string name = Path.GetFileName(path);
                            try
                            {
                                File.Copy(path, Environment.CurrentDirectory + name);                                
                            }
                            catch(IOException ex)
                            {
                                MessageBox.Show("File with the same name exist in App Folder");                                
                            }         
                            finally
                            {
                                _item.Add(Environment.CurrentDirectory + name);
                            }  
                            
                            
                            /* I dont know why I wrote it this way...
                             * Simple way:
                             * if(!File.Exist(path)
                             * {
                             *    File.Copy(path, Environment.CurrentDirectory + name);
                             * }                            
                             */                
                            
                        }
                        break;
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
            userAction = action;

            switch (userAction)
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

                case Enums.Action.Previous:
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
                        goto case Enums.Action.Previous;
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
