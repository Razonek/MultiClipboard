namespace Multi_Clipboard
{
    public class Enums
    {
        /// <summary>
        /// Virtual Key State
        /// </summary>
        public enum Key
        {
            Ctrl = 0x11,
            Alt = 0xA4,  //left alt
            Shift = 0x10,
            C = 0x43,
            V = 0x56,
            X = 0x58,
        }

        public enum ItemType
        {
            Text,
            Music,
            Picture,
            Video,
            OtherObject,
        }

        public enum Action
        {
            Next,
            Previous,
            Copy,
            Cut,
            ClearAll,
            Delete,
            None,
        }

        public enum KeyType
        {
            NextSpecial,
            Next,
            PreviousSpecial,
            Previous,
        }



    }
}
