using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Interfaces;
using System;
using System.Data;

namespace Maestro.BaseLibrary.Entities
{
    public class SummaryRegion : ISummaryRegion
    {
        //IBaseRegion property variables
        private string _displayName1;
        private string _displayName2;
        private bool _mainRegion;
        private string _mapCode;
        private int _regionID;
        private string _regionName;
        private int _totalRidings;

        //ISummaryRegionData property variables
        private int _currentElected;
        private int _currentLeading;
        private int _currentTotalVotes;
        private int _currentVotes;
        private float _currentVoteShare;
        private int _previousTotalVotes;
        private int _previousVotes;
        private float _previousVoteShare;
        private int _prevReportingSeats;

        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName1; } }
        public bool MainRegion { get { return _mainRegion; } }
        public string MapCode { get { return _mapCode; } }
        public int RegionID { get { return _regionID; } }
        public string RegionName { get { return _regionName; } }
        public int TotalRidings { get { return _totalRidings; } }
        public int CurrentElected { get { return _currentElected; } }
        public int CurrentLeading { get { return _currentLeading; } }
        public int CurrentTotalVotes { get { return _currentTotalVotes; } }
        public int CurrentVotes { get { return _currentVotes; } }
        public float CurrentVoteShare { get { return _currentVoteShare; } }
        public int PreviousTotalVotes { get { return _previousTotalVotes; } }
        public int PreviousVotes { get { return _previousVotes; } }
        public float PreviousVoteShare { get { return _previousVoteShare; } }
        public int PrevReportingSeats { get { return _prevReportingSeats; } }

        /// <summary>
        /// Instantiate a new SummaryRegionData object from a BaseRegion object
        /// </summary>
        /// <param name="baseRegion"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SummaryRegion Create(BaseRegion baseRegion)
        {
            SummaryRegion r = new SummaryRegion();
            r._displayName1 = baseRegion.DisplayName1;
            r._displayName2 = baseRegion.DisplayName2;
            r._mainRegion = baseRegion.MainRegion;
            r._mapCode = baseRegion.MapCode;
            r._regionID = baseRegion.RegionID;
            r._regionName = baseRegion.RegionName;
            r._totalRidings = baseRegion.TotalRidings;
            return r;
        }

        /// <summary>
        /// Assign current election values (various vote totals) to an existing SummaryRegionData object
        /// </summary>
        /// <param name="row"></param>
        /// <remarks></remarks>
        public void UpdateValues(DataRow row)
        {
            _currentLeading = 0;
            _currentElected = Int32.Parse(row["PartyTotSeats"].ToString());
            _currentVotes = Int32.Parse(row["PartyTotVotes"].ToString());
            _prevReportingSeats = Int32.Parse(row["PartyTotPrevElectActRid"].ToString());
            _previousVotes = Int32.Parse(row["PartyTotVotesPrevElect"].ToString());
        }

        /// <summary>
        /// Assign overall vote totals for previous and current election and calculate the vote share
        /// values for both elections
        /// </summary>
        /// <param name="regionTotalVotes"></param>
        /// <param name="regionTotalVotesPrevElection"></param>
        /// <remarks></remarks>
        public void UpdateVoteShareValues(int regionTotalVotes, int regionTotalVotesPrevElection)
        {
            _previousTotalVotes = regionTotalVotesPrevElection;
            if (_previousTotalVotes > 0)
            {
                _previousVoteShare = ((_previousVotes / (float)_previousTotalVotes) * 100);
            }
            else
            {
                _previousVoteShare = 0;
            }
            _currentTotalVotes = regionTotalVotes;
            if (_currentTotalVotes > 0)
            {
                _currentVoteShare = ((_currentVotes / (float)_currentTotalVotes) * 100);
            }
            else
            {
                _currentVoteShare = 0;
            }
        }
    }

}
