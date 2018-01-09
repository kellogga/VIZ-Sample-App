using Maestro.BaseLibrary.DataObjects;
using Maestro.BaseLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Maestro.BaseLibrary.BaseObjects
{
    public class BaseRiding: IBaseRiding
    {
        private int _ridingID;
        private int _ridingNumber;
        private string _provinceCode;
        private string _ridingName;
        private string _displayName1;
        private string _displayName2;
        private int _incumbentPartyID;
        private int _totalPolls;
        private int _totalVoters;
        private int _prevTotalVoters;
        private bool _heroNoteShown;
        private string _frComments;
        private List<BaseCandidate> _baseCandidates = new List<BaseCandidate>();

        public int RidingID { get { return _ridingID; } }
        public int RidingNumber { get { return _ridingNumber; } }
        public string ProvinceCode { get { return _provinceCode; } }
        public string RidingName { get { return _ridingName; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }
        public int IncumbentPartyID { get { return _incumbentPartyID; } }
        public int TotalPolls { get { return _totalPolls; } }
        public int TotalVoters { get { return _totalVoters; } }
        public int PrevTotalVoters { get { return _prevTotalVoters; } }
        public string FrComments { get { return _frComments; } }
        public List<BaseCandidate> BaseCandidates { get { return _baseCandidates; } }
        public bool HeroNoteShown { get { return _heroNoteShown; } set { _heroNoteShown = value; } }

        /// <summary>
        /// Factory method to instantiate one BaseRiding object from DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static BaseRiding Create(DataRow row, List<Map> maps, int mediaId)
        {
            BaseRiding r = new BaseRiding();
            try
            {
                //set the properties passed into the method
                r._ridingID = Int32.Parse(row["RidingId"].ToString());
                r._ridingNumber = Int32.Parse(row["RidingNumber"].ToString());
                foreach (Map m in maps)
                {
                    if (m.StartRiding > 0)
                    {
                        if (r._ridingNumber >= m.StartRiding & r._ridingNumber <= m.EndRiding)
                        {
			                r._provinceCode = m.ProvCode;
                            break;
                        }
                    }
                }
                if (mediaId == 1)
                {
                    r._ridingName = row["EnglishName"].ToString();
                }
                else
                {
                    if (row.Table.Columns.Contains("FrenchName"))
                    {
                        r._ridingName = row["FrenchName"].ToString();
                    }
                    else
                    {
                        r._ridingName = row["EnglishName"].ToString();
                    }
                }
                r._displayName1 = BaseUtilities.ProperCase(r._ridingName);
                r._displayName2 = r._displayName1;
                r._totalPolls = Int32.Parse(row["TotalPolls"].ToString());
                r._totalVoters = Int32.Parse(row["TotalVoters"].ToString());
                r._prevTotalVoters = Int32.Parse(row["PreviousElectionTotalVoters"].ToString());
                r._incumbentPartyID = Int32.Parse(row["PreviousElectedPartyID"].ToString());
                r._frComments = row["FrenchComments"].ToString();
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public BaseCandidate Item(int id)
        {
            foreach (BaseCandidate c in _baseCandidates)
            {
                if (c.CandidateID == id)
                {
                    return c;
                }
            }
            return null;
        }

        public BaseCandidate Candidate(int id)
        {
            {
                foreach (BaseCandidate c in _baseCandidates)
                {
                    if (c.CandidateID == id)
                    {
                        return c;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Figure out which type of display name we have and assign to the appropriate property
        /// </summary>
        /// <param name="row"></param>
        public void UpdateDisplayName(DataRow row)
        {
            if (string.Compare(row["TypeName"].ToString(), "Ridings1", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName1 = row["DisplayName"].ToString();
            }
            if (string.Compare(row["TypeName"].ToString(), "Ridings2", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName2 = row["DisplayName"].ToString();
            }
        }

        /// <summary>
        /// Override the ToString() method of this object to return the party code, not the object type.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _ridingName.ToUpper() + " (" + _ridingNumber + ")";
        }
    }
}
