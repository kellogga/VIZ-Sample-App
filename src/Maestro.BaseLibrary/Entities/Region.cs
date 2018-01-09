using System;
using System.Collections.Generic;
using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Interfaces;
using System.Globalization;
using Maestro.BaseLibrary.Config;
using VizWrapper;

namespace Maestro.BaseLibrary.Entities
{
    public class Region : IBaseBoard, IRegion
    {
        private string _boardKey;
        private string _graphicName;
        private string _datapoolName;
        private string _channelName;
        private bool _showOthers;
        private bool _calculateVoteShare;
        private int _itemsPerPage;
        private int _maxItemsPerPage;
        private int _numberOfPages;
        private int _currentPageNumber;
        private string _electionType;
        private int _firstItem;
        private int _numberOfItemsToDisplay;
        private int _pauseInterval;
        private DataType _typeOfData;

        private int _regionID;
        private string _regionName;
        private string _displayName1;
        private string _displayName2;
        private bool _mainRegion;
        private string _mapCode;
        private BaseParty _desiredParty;

        private List<Riding> _ridings = new List<Riding>();
        private int _currentTotalVotes;
        private int _previousTotalVotes;
        private int _totalRidings;
        private int _reportingRidings;
        private string _miniMessage;
        private int _pollsReported;
        private int _totalPolls;
        private List<Party> _parties = new List<Party>();

        public string BoardKey { get { return _boardKey; } }
        public string GraphicName { get { return _graphicName; } }
        public string DatapoolName { get { return _datapoolName; } }
        public string ChannelName { get { return _channelName; } }
        public int TotalItems
        {
            get
            {
                if (_parties.Count > _itemsPerPage & string.Compare(_boardKey, "Vote Change", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                {
                    //We don't show 'others' on the Vote Share board but we don't want to display
                    //more than one page either.  Force TotalItems to ItemsPerPage instead of the 
                    //actual number of parties with votes which could be over 15 in a federal election.
                    return _itemsPerPage;
                }
                else
                {
                    return _parties.Count;
                }
            }
        }
        public bool ShowOthers { get { return _showOthers; } }
        public bool CalculateVoteShare { get { return _calculateVoteShare; } }
        public int ItemsPerPage { get { return _itemsPerPage; } }
        public int MaxItemsPerPage { get { return _maxItemsPerPage; } }
        public int NumberOfPages
        {
            get
            {
                if (_desiredParty != null)
                {
                    return 1;
                }
                else
                {
                    //Figure out how many pages we have...
                    _numberOfPages = TotalItems / _itemsPerPage;
                    if (TotalItems % _itemsPerPage > 0)
                    {
                        _numberOfPages += 1;
                    }
                    //Special limit requested by Mark Bulgutch Sept 29, 2008.  For Underbar playlist types
                    //Party Standings and Share of Vote, he only wants to see the first 2 pages of data, even
                    //though there may be more than 2 pages.
                    if (string.Compare(_boardKey, "UB Party Standings", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0 | string.Compare(_boardKey, "UB Vote Share", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                    {
                        if (_numberOfPages > 2)
                            _numberOfPages = 2;
                    }
                    return _numberOfPages;
                }
            }
        }
        public int CurrentPageNumber { get { return _currentPageNumber; } }
        public string ElectionType { get { return _electionType; } }
        public int FirstItem { get { return _firstItem; } }
        public int NumberOfItemsToDisplay { get { return _numberOfItemsToDisplay; } }
        public int PauseInterval { get { return _pauseInterval; } }
        public DataType TypeOfData { get { return _typeOfData; } }
        public void SetPageNumber(int pageNumber)
        {
            _currentPageNumber = pageNumber;
            if (_currentPageNumber * _itemsPerPage <= TotalItems)
            {
                _numberOfItemsToDisplay = _itemsPerPage;
            }
            else
            {
                _numberOfItemsToDisplay = TotalItems % _itemsPerPage;
            }
            _firstItem = (_currentPageNumber - 1) * _itemsPerPage;
        }

        public int RegionID { get { return _regionID; } }
        public string RegionName { get { return _regionName; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }
        public bool MainRegion { get { return _mainRegion; } }
        public string MapCode { get { return _mapCode; } }
        public BaseParty DesiredParty { get { return _desiredParty; }  }
        public List<Riding> Ridings  { get { return _ridings; } }
        public int CurrentTotalVotes { get { return _currentTotalVotes; } set { _currentTotalVotes = value; } }
        public int PreviousTotalVotes { get { return _previousTotalVotes; } set { _previousTotalVotes = value; } }
        public int TotalRidings { get { return _totalRidings; } }
        public int ReportingRidings { get { return _reportingRidings; } set { _reportingRidings = value; } }
        public string MiniMessage { get { return _miniMessage; } }
        public int PollsReported { get { return _pollsReported; } }
        public int TotalPolls { get { return _totalPolls; } }
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

        public Party GetPartyByID(int id)
        {
            foreach (Party p in _parties)
            {
                if (p.PartyID == id)
                {
                    return p;
                }
            }
            return null;
        }
        public void SetDataPoolName(string newName)
        {
            _datapoolName = newName;
        }

        public void SetPollsData(int pollsReported, int totalPolls)
        {
            _pollsReported = pollsReported;
            _totalPolls = totalPolls;
        }

        /// <summary>
        /// Assign dynamic data to our BaseRegion object
        /// </summary>
        /// <param name="baseRegion"></param>
        /// <param name="scene"></param>
        /// <param name="mainParties"></param>
        /// <param name="electionType">'Federal', 'Provincial', 'Municipal' etc.</param>
        /// <param name="miniMessage"></param>
        /// <param name="desiredParty"></param>
        public void Update(BaseRegion baseRegion, Scene scene, int mainParties, string electionType, string miniMessage = "", BaseParty desiredParty = null)
        {
            //Assign properties from base object
            _displayName1 = baseRegion.DisplayName1;
            _displayName2 = baseRegion.DisplayName2;
            _mainRegion = baseRegion.MainRegion;
            _mapCode = baseRegion.MapCode;
            _regionID = baseRegion.RegionID;
            _regionName = baseRegion.RegionName;
            _totalRidings = baseRegion.TotalRidings;

            //Assign properties from scene object
            _boardKey = scene.Name;
            _graphicName = scene.InternalName;
            _datapoolName = scene.DatapoolName;
            _showOthers = scene.ShowOthers;
            _calculateVoteShare = scene.CalculateVoteShare;
            _channelName = scene.ChannelName;
            _itemsPerPage = Int32.Parse((scene.ShowOthers ? mainParties + 1 : scene.NumberOfItems).ToString());
            _maxItemsPerPage = scene.NumberOfItems;
            _typeOfData = scene.TypeOfData;
            _pauseInterval = scene.PauseInterval;

            //Initialize other properties required by board logic
            _desiredParty = desiredParty;
            _currentPageNumber = 0;
            _electionType = electionType;
            _miniMessage = miniMessage;
        }
        public void UpdateItemPerPage(int itemPerPage)
        {
            //Assign item per page to scene object
            _itemsPerPage = itemPerPage;
        }

        public void UpdateWomanSeats(WomanSeats womanSeats, List<BaseParty> baseParties)
        {
            //Initially, set all previous seat counts back to 0 for all parties
            foreach (Party p in _parties)
            {
                p.UpdateWomanSeat(0);
            }
            //Now, iterate through the previous election array and assign any values for each
            //party to the previous seat count value
            foreach (WomanSeat w in womanSeats)
            {
                bool foundParty = false;
                foreach (Party p in _parties)
                {
                    if (string.Compare(p.PartyCode, w.PartyCode) == 0)
                    {
                        p.UpdateWomanSeat(Int32.Parse(w.SeatCount));
                        foundParty = true;
                        break; 
                    }
                }
                if (!foundParty)
                {
                    //We have a party that had elected female members of parliament at disolution
                    //but we don't have any currently leading or elected female candidates
                    //for this party in the current election.  Add an entry to this region's 
                    //party collection for this party and assign the previous seat count
                    foreach (BaseParty bp in baseParties)
                    {
                        if (string.Compare(bp.PartyCode, w.PartyCode) == 0)
                        {
                            _parties.Add(Party.Create(bp));
                            Party newP = _parties[_parties.Count - 1];
                            newP.UpdateWomanSeat(Int32.Parse(w.SeatCount));
                            break; 
                        }
                    }
                }
            }
        }
        public bool PartyExists(int Id)
        {
            foreach (Party p in _parties)
            {
                if (p.PartyID == Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
