using System;
using System.Data;

namespace Maestro.BaseLibrary.Entities
{
    class RidingResult
    {
        private int _totalVotes;
        private int _pollsReporting;
        private DateTime _resultTime;
        private string _candidateVotes;
        private int _resultStatus;

        public int TotalVotes { get { return _totalVotes; } }
        public int PollsReporting { get { return _pollsReporting; } }
        public DateTime ResultTime { get { return _resultTime; } }
        public string CandidateVotes { get { return _candidateVotes; } }
        public int ResultStatus { get { return _resultStatus; } }
        public static RidingResult Create(DataRow row)
        {
            RidingResult r = new RidingResult();
            try
            {
                r._totalVotes = Int32.Parse(row["TotalVotes"].ToString());
                r._pollsReporting = Int32.Parse(row["PollsReporting"].ToString());
                r._resultTime = DateTime.Parse(row["ResultTime"].ToString());
                r._candidateVotes = row["CandidateVotes"].ToString();
                r._resultStatus = Int32.Parse(row["ResultStatus"].ToString());
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
