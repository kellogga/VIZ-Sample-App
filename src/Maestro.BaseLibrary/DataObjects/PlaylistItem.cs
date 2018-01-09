using System;
using System.Data;

namespace Maestro.BaseLibrary.DataObjects
{
    public class PlaylistItem
    {
        private int _playlistItemID;
        private int _elementID;
        private string _elementName;
        private string _partyCode;
        private string _boardName;

        public int PlaylistItemID { get { return _playlistItemID; } }
        public int ElementId { get { return _elementID; } }
        public string ElementName { get { return _elementName; } }
        public string PartyCode { get { return _partyCode; } }
        public string BoardName { get { return _boardName; } }

        public static PlaylistItem Create(DataRow row)
        {
            PlaylistItem p = new PlaylistItem();
            try
            {
                //set the properties passed into the method
                p._playlistItemID = Int32.Parse(row["PlaylistItemID"].ToString());
                p._elementID = Int32.Parse(row["ElementID"].ToString());
                p._elementName = row["ElementName"].ToString();
                p._partyCode = row["PartyCode"].ToString();
                p._boardName = row["BoardName"].ToString();
                return p;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override string ToString()
        {
            if (_partyCode.Length > 0)
            {
                return _boardName + ": " + _elementName + ": " + _partyCode;
            }
            else
            {
                return _boardName + ": " + _elementName;
            }
        }
    }
}
