using System;
using System.Collections.Generic;
using System.Data;

namespace Maestro.BaseLibrary.DataObjects
{
    public class Playlist
    {
        private int _playlistID;
        private string _playlistName;
        private List<PlaylistItem> _playlistItems = new List<PlaylistItem>();
        
        public int PlaylistID { get { return _playlistID; } }
        public string PlaylistName { get { return _playlistName; } }
        public string DisplayName { get { return _playlistID.ToString() + " - " + _playlistName; } }
        public List<PlaylistItem> PlaylistItems { get { return _playlistItems; } }
        
        public static Playlist Create(DataRow row)
        {
            Playlist p = new Playlist();
            try
            {
                //set the properties passed into the method
                p._playlistID = Int32.Parse(row["PlaylistItemID"].ToString());
                p._playlistName = row["ElementName"].ToString();
                return p;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal void Add(PlaylistItem playlistItem)
        {
            _playlistItems.Add(playlistItem);
        }
    }
}
