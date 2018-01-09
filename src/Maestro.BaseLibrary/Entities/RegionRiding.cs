using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Interfaces;
using System.Collections.Generic;
using System;
using Maestro.BaseLibrary.Config;
using VizWrapper;

namespace Maestro.BaseLibrary.Entities
{
    public class RegionRiding: IBaseBoard, IRegionRiding
    {
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

        private int _regionID;
        private string _regionName;
        private string _displayName1;
        private string _displayName2;
        private bool _mainRegion;
        private string _mapCode;
        private BaseParty _desiredParty;
        private List<Riding> _ridings = new List<Riding>();

        public string ChannelName { get { return _channelName; } }
        public string ElectionType { get { return _electionMode; } }
        public int ItemsPerPage { get { return _itemsPerPage; } }
        public int MaxItemsPerPage { get { return _maxItemsPerPage; } }
        public bool ShowOthers { get { return _showOthers; } }
        public int NumberOfPages
        {
            get
            {
                //Figure out how many pages we have...
                _numberOfPages = _ridings.Count / _itemsPerPage;
                if (_ridings.Count % _itemsPerPage > 0)
                {
                    _numberOfPages += 1;
                }
                return _numberOfPages;
            }
        }
        public int TotalItems { get { return _ridings.Count; } }
        public bool CalculateVoteShare { get { return _calculateVoteShare; } }
        public int CurrentPageNumber { get { return _currentPageNumber; } }
        public string GraphicName { get { return _graphicName; } }
        public string DatapoolName { get { return _datapoolName; } }
        public string BoardKey { get { return _boardKey; } }
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
        public BaseParty DesiredParty { get { return _desiredParty; } }

        public List<Riding> Ridings { get { return _ridings; } }

        public int TotalRidings { get { return _ridings.Count; } }

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

        public void Update(BaseRegion baseRegion, Scene scene, int mainParties, string electionMode, BaseParty desiredParty = null)
        {
            //Assign properties from base object
            _displayName1 = baseRegion.DisplayName1;
            _displayName2 = baseRegion.DisplayName2;
            _mainRegion = baseRegion.MainRegion;
            _mapCode = baseRegion.MapCode;
            _regionID = baseRegion.RegionID;
            _regionName = baseRegion.RegionName;

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
            _electionMode = electionMode;

        }

        /// <summary>
        /// Make sure the candidates are sorted in the proper order
        /// (primarily by votes and secondarily by party priority)
        /// </summary>
        public void SortCandidates()
        {
            foreach (Riding r in _ridings)
            {
                r.SortCandidates();
            }
        }
    }
}
