using System;
using Maestro.BaseLibrary.Interfaces;
using System.Data;
using System.Windows.Forms;
using Maestro.BaseLibrary.BaseObjects;
using System.Collections.Generic;

namespace Maestro.BaseLibrary.Entities
{
    public class Party : IParty
    {
        private int _partyID;
        private int _partyPriority;
        private string _partyCode;
        private string _partyName;
        private string _displayName1;
        private string _displayName2;
        private int _currentElectedSeats;
        private int _currentLeadingSeats;
        private int _currentVotes;
        private int _prevReportingSeats;
        private int _prevNonReportingSeats;
        private int _previousVotes;

        public int PartyID { get { return _partyID; } }
        public int PartyPriority { get { return _partyPriority; } }
        public string PartyCode { get { return _partyCode; } }
        public string PartyName { get { return _partyName; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }
        public int CurrentElectedSeats { get { return _currentElectedSeats; } }
        public int CurrentLeadingSeats { get { return _currentLeadingSeats; } }
        public int CurrentVotes { get { return _currentVotes; } }
        public int PrevReportingSeats { get { return _prevReportingSeats; } }
        public int PrevNonReportingSeats { get { return _prevNonReportingSeats; } }
        public int PreviousVotes { get { return _previousVotes; } }

        /// <summary>
        /// Factory method to create one Party object from a BaseParty object
        /// </summary>
        /// <param name="baseParty"></param>
        /// <returns></returns>
        public static Party Create(BaseParty baseParty)
        {
            Party p = new Party();
            p._partyID = baseParty.PartyID;
            p._partyCode = baseParty.PartyCode;
            p._partyName = baseParty.PartyName;
            p._displayName1 = baseParty.DisplayName1;
            p._displayName2 = baseParty.DisplayName2;
            p._partyPriority = baseParty.PartyPriority;
            return p;
        }

        /// <summary>
        /// Factory method to create one Party object from a DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Party Create(DataRow row)
        {
            Party p = new Party();
            p._partyID = Int32.Parse(row["PAR_ID"].ToString());
            p._partyCode = row["PAR_CODE_EN"].ToString();
            p._partyName = row["ENGNAME"].ToString();
            p._displayName1 = p._partyName;
            p._displayName2 = p._partyName;
            p._partyPriority = Int32.Parse(row["sPriority"].ToString());
            return p;
        }

        /// <summary>
        /// Factory method to create one party data object from a DataGridViewRow object (manual mode operation for the mini)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Party Create(DataGridViewRow row)
        {
            Party p = new Party();
            p._partyID = Convert.ToInt32(row.Cells["ID"].Value);
            p._partyPriority = Convert.ToInt32(row.Cells["Priority"].Value);
            p._partyCode = Convert.ToString(row.Cells["PartyCode"].Value);
            p._partyName = Convert.ToString(row.Cells["Party"].Value);
            p._currentLeadingSeats = Convert.ToInt32(row.Cells["Seats"].Value);
            return p;
        }

        /// <summary>
        /// Update the dynamic data for this party
        /// </summary>
        /// <param name="row"></param>
        public void Update(DataRow row)
        {
            _currentLeadingSeats = Int32.Parse(row["PartyTotLead"].ToString());
            _currentElectedSeats = Int32.Parse(row["PartyTotElect"].ToString());
            _currentVotes = Int32.Parse(row["PartyTotVotes"].ToString());
            _prevReportingSeats = Int32.Parse(row["PartyTotPrevLeadActRid"].ToString());
            _prevNonReportingSeats = Int32.Parse(row["PrevNonReportingSeats"].ToString());
            _previousVotes = Int32.Parse(row["PartyTotVotesPrevElect"].ToString());
        }

        public void UpdateWomanSeat(int seatCount)
        {
            _prevReportingSeats = seatCount;
        }

        /// <summary>
        /// Override the ToString method to return the party code, not eh object type
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _partyCode.ToUpper();
        }

        internal void AddValues(Party p)
        {
            _currentVotes += p.CurrentVotes;
            _previousVotes += p.PreviousVotes;
            _currentElectedSeats += p.CurrentElectedSeats;
            _currentLeadingSeats += p.CurrentLeadingSeats;
            _prevReportingSeats += p.PrevReportingSeats;
        }
    }

    /// <summary>
    /// Helper class used to sort list of Party objects by total # of leading and elected seats
    /// </summary>
    internal class PartyComparerSeats : IComparer<Party>
    {
        public int Compare(Party p1, Party p2)
        {
            //For legibility
            int p1Seats = p1.CurrentLeadingSeats + p1.CurrentElectedSeats;
            int p2Seats = p2.CurrentLeadingSeats + p2.CurrentElectedSeats;

            if (p1Seats != p2Seats)
            {
                //Sort the parties, primarily by # of leading & elected seats.
                if (p1Seats < p2Seats)
                {
                    return 1;
                }
                if (p1Seats > p2Seats)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                //We've got a tie so use Party Priority as our sort criteria
                if (p1.PartyPriority < p2.PartyPriority)
                {
                    return -1;
                }
                if (p1.PartyPriority > p2.PartyPriority)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// Helper class used to sort list of Party objects by total party votes
    /// </summary>
    internal class PartyComparerVotes : IComparer<Party>
    {
        public int Compare(Party p1, Party p2)
        {
            if (p1.CurrentVotes != p2.CurrentVotes)
            {
                //Sort the parties, primarily by # of votes.
                if (p1.CurrentVotes < p2.CurrentVotes)
                {
                    return 1;
                }
                if (p1.CurrentVotes > p2.CurrentVotes)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                //We've got a tie so use Party Priority as our sort criteria
                if (p1.PartyPriority < p2.PartyPriority)
                {
                    return -1;
                }
                if (p1.PartyPriority > p2.PartyPriority)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

}
