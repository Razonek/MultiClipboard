using System.Collections.Specialized;


namespace Multi_Clipboard
{
    public class Item
    {
        public Item(StringCollection item, bool isText)
        {
            this.item = item;
            this.isText = isText;
        }

        public StringCollection item { get; private set; }
        public bool isText { get; private set; }
    }
}
