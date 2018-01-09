using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Config;
using System;
using System.Collections.Generic;
using VizWrapper;

namespace Maestro.BaseLibrary.Entities
{
    class NoQuery: IBaseBoard
    {
        //IBaseBoard property variables
        private string _channelName;
        private string _electionMode;
        private int _itemsPerPage;
        private int _maxItemsPerPage;
        private bool _showOthers;
        private int _numberOfPages;
        private bool _calculateVoteShare;
        private int _currentPageNumber;
        private string _graphicName;
        private string _datapoolName;
        private string _boardKey;
        private int _firstItem;
        private int _numberOfItemsToDisplay;
        private int _pauseInterval;
        private DataType _typeOfData;

        private BaseParty _party;
        private string _boardText;
        private string _boardText2;
        private string _boardText3;
        private List<Party> _parties = new List<Party>();

        public string ChannelName { get { return _channelName; } }
        public string ElectionType { get { return _electionMode; } }
        public int ItemsPerPage { get { return _itemsPerPage; } }
        public int MaxItemsPerPage { get { return _maxItemsPerPage; } }
        public bool ShowOthers { get { return _showOthers; } }
        public int NumberOfPages { get { return _numberOfPages; } }
        public bool CalculateVoteShare { get { return _calculateVoteShare; } }
        public int CurrentPageNumber { get { return _currentPageNumber; } }
        public string GraphicName { get { return _graphicName; } }
        public string DatapoolName { get { return _datapoolName; } }
        public string BoardKey { get { return _boardKey; } }
        public int FirstItem { get { return _firstItem; } }
        public int NumberOfItemsToDisplay { get { return _numberOfItemsToDisplay; } }
        public int PauseInterval { get { return _pauseInterval; } }
        public DataType TypeOfData { get { return _typeOfData; } }
        public int TotalItems { get { return 1; } }
        public void SetPageNumber(int pageNumber)
        {
            _currentPageNumber = 1;
        }
        public void SetPartyOrder(Region region)
        {
            int index = 0;
            _parties = new List<Party>();
            foreach (Party p in region.Parties)
            {
                index += 1;
                if (index > 5)
                {
                    break; 
                }
                if (p.PartyPriority < 50)
                {
                    _parties.Add(p);
                }
            }
        }
        public BaseParty Party { get { return _party; } }
        public string BoardText { get { return _boardText; } }
        public string BoardText2 { get { return _boardText2; } }
        public string BoardText3 { get { return _boardText3; } }
        public List<Party> Parties { get { return _parties; } }

        public Scene Scene
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IScene IBaseBoard.Scene
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Update(Scene scene, BaseParty party, string electionMode, string boardText)
        {
            //Assign properties from scene object
            _channelName = scene.ChannelName;
            _boardKey = scene.Name;
            _graphicName = scene.InternalName;
            _datapoolName = scene.DatapoolName;
            _itemsPerPage = scene.NumberOfItems;
            _maxItemsPerPage = scene.NumberOfItems;
            _typeOfData = scene.TypeOfData;

            //Assign other values
            _party = party;
            _calculateVoteShare = false;
            _currentPageNumber = 1;
            _numberOfPages = 1;
            _electionMode = electionMode;
            _pauseInterval = scene.PauseInterval;
            if (boardText.Contains("|"))
            {
                string[] boardTextArray = boardText.Split(Convert.ToChar("|"));
                _boardText = boardTextArray[0];
                _boardText2 = boardTextArray[1];
                _boardText3 = boardTextArray[2];
            }
            else
            {
                _boardText = boardText;
                _boardText2 = "";
                _boardText3 = "";
            }
        }
        public void Update(string datapoolName)
        {
            _channelName = datapoolName;
            _boardKey = datapoolName;
            _graphicName = datapoolName;
            _datapoolName = datapoolName;
            _itemsPerPage = 1;
            _maxItemsPerPage = 1;
            _typeOfData = DataType.None;

            //Assign other values
            _party = null;
            _calculateVoteShare = false;
            _currentPageNumber = 1;
            _numberOfPages = 1;
            _electionMode = "Federal";
            _boardText = "";
        }

    }
}
