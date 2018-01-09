using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Config;
using Maestro.BaseLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using VizWrapper;

namespace Maestro.BaseLibrary.Entities
{
    public class Summary : IBaseBoard, ISummary
    {
        //IBaseBoard property variables
        private int _itemsPerPage;
        private int _numberOfPages;
        private int _currentPageNumber;
        private int _firstItem;
        private int _numberOfItemsToDisplay;
        private Scene _scene;

        //IBaseSummary property variables
        private string _displayName1;
        private string _displayName2;
        private int _summaryID;

        private string _summaryName;
        //ISummaryData property variables
        private BaseParty _party;
        private List<SummaryRegion> _summaryRegions = new List<SummaryRegion>();

        public int CurrentPageNumber { get { return _currentPageNumber; } }
        public int ItemsPerPage { get { return _itemsPerPage; } }
        public int NumberOfPages
        {
            get {  
                //Figure out how many pages we have for this board...
                if (string.Compare(_scene.Name, "Party Strength", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                {
                    //The party strength board always has 2 pages - it is not dependant
                    //on the itemsPerPage value
                    _numberOfPages = 2;
                }
                else
                {
                    _numberOfPages = _summaryRegions.Count / _itemsPerPage;
                    if (_summaryRegions.Count % _itemsPerPage > 0)
                    {
                        _numberOfPages += 1;
                    }
                }
                return _numberOfPages;
            }
        }

        public int TotalItems { get { return _summaryRegions.Count - 1; } }
        public int FirstItem { get { return _firstItem; } }
        public int NumberOfItemsToDisplay { get { return _numberOfItemsToDisplay; } }
        public Scene Scene { get { return _scene; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }
        public int SummaryID { get { return _summaryID; } }
        public string SummaryName { get { return _summaryName; } }
        public BaseParty Party { get { return _party; } }
        public List<SummaryRegion> SummaryRegions { get { return _summaryRegions; } }

        public string ElectionType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PauseInterval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DatapoolName
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

        /// <summary>
        /// Implementation of base object's SetPageNumber method.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <remarks>Recalculates the 'number of items to display' and the 'first item' values</remarks>
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
            _firstItem = ((_currentPageNumber - 1) * _itemsPerPage) + 1;
        }

        /// <summary>
        /// Assign dynamic data to object
        /// </summary>
        /// <param name="baseSummary"></param>
        /// <param name="scene"></param>
        /// <param name="desiredParty"></param>
        /// <remarks></remarks>

        public void Update(BaseSummary baseSummary, Scene scene, int mainParties, string electionMode, BaseParty party = null)
        {
            //Assign properties from base object
            _displayName1 = baseSummary.DisplayName1;
            _displayName2 = baseSummary.DisplayName2;
            _summaryID = baseSummary.SummaryID;
            _summaryName = baseSummary.SummaryName;
            _scene = scene;
            _itemsPerPage = Int32.Parse((scene.ShowOthers ? mainParties + 1 : scene.NumberOfItems).ToString());
            _summaryRegions.Clear();

            //Initialize other properties required by board logic
            _party = party;
            _currentPageNumber = 0;
        }
  
    }
}
