using System;
using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Interfaces;
using System.Data;
using System.Collections.Generic;

namespace Maestro.BaseLibrary.Entities
{
    public class Candidate : ICandidate
    {
        private int _candidateID;
        private BaseParty _party;
        private string _firstName;
        private string _lastName;
        private string _displayName1;
        private string _displayName2;
        private int _candidateType;
        private string _heroNote;
        private int _status;
        private string _candidateStatus;
        private int _previousVotes;
        private string _gender;
        private string _candidateNote1;
        private string _candidateNote2;
        private bool _isRenegade;
        private int _votes;
        private int _lead;
        private int _rank;

        public int CandidateID { get { return _candidateID; } }
        public string CandidateNote1 { get { return _candidateNote1; } }
        public string CandidateNote2 { get { return _candidateNote2; } }
        public string CandidateStatus { get { return _candidateStatus; } }
        public int CandidateType { get { return _candidateType; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }
        public string FirstName { get { return _firstName; } }
        public string Gender { get { return _gender; } }
        public string HeroNote { get { return _heroNote; } }
        public bool IsRenegade { get { return _isRenegade; } }
        public string LastName { get { return _lastName; } }
        public int Lead { get { return _lead; } }
        public BaseParty Party { get { return _party; } }
        public int PreviousVotes { get { return _previousVotes; } }
        public int Rank { get { return _rank; } }
        public int Status { get { return _status; } }
        public int Votes { get { return _votes; } }


        /// <summary>
        /// Factory method to instantiate a new Candidate object from a BaseCandidate
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        public static Candidate Create(BaseCandidate bc)
        {
            Candidate c = new Candidate();
            c._candidateID = bc.CandidateID;
            c._candidateType = bc.CandidateType;
            c._displayName1 = bc.DisplayName1;
            c._displayName2 = bc.DisplayName2;
            c._heroNote = bc.HeroNote;
            c._firstName = bc.FirstName;
            c._lastName = bc.LastName;
            c._party = bc.Party;
            c._previousVotes = bc.PreviousVotes;
            c._gender = bc.Gender;
            c._candidateNote1 = bc.CandidateNote1;
            c._candidateNote2 = bc.CandidateNote2;
            c._isRenegade = bc.IsRenegade;
            return c;
        }

        /// <summary>
        /// Override the default ToString to return a nicely formatted description of the candidate
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _party.PartyCode + "\t" + _votes.ToString("N0").PadLeft(7) + "\t" + _lastName;
        }

        /// <summary>
        /// Dynamically assign the candidate's rank during an election
        /// </summary>
        /// <param name="rank"></param>
        public void SetRank(int rank)
        {
            _rank = rank;
        }

        public void SetVotes(int votes)
        {
            _votes = votes;
        }

        /// <summary>
        /// Dynamically set the status of the candidate during an election
        /// </summary>
        /// <param name="status"></param>
        public void SetCandidateStatus(string status)
        {
            _candidateStatus = status;
        }

        /// <summary>
        /// Dynamically update this candidate's current vote counts and lead value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="firstCandidate"></param>
        /// <param name="leadVotes"></param>
        /// <returns></returns>
        public int UpdateValues(DataRow row, bool firstCandidate, int leadVotes)
        {
            int retval = leadVotes;
            if (firstCandidate)
            {
                //For the first candidate, set their leading value and keep track of
                //their actual votes so we can calculate how much each other candidate
                //is trailing by
                _lead = Int32.Parse(row["DDIPCandidateVotesLead"].ToString());
                retval = Int32.Parse(row["DDIPVotes"].ToString());
            }
            else
            {
                //Keep track of how many votes this candidate trails the leader
                _lead = Int32.Parse(row["DDIPVotes"].ToString()) - leadVotes;
            }
            _votes = Int32.Parse(row["DDIPVotes"].ToString());
            _status = Int32.Parse(row["DDIPStatus"].ToString());
            return retval;
        }
    }

    /// <summary>
    /// Helper class used to sort list of Candidates when there are no results
    /// </summary>
    internal class CandidateComparerNoResults : IComparer<Candidate>
    {
        public int Compare(Candidate c1, Candidate c2)
        {
            //Always sort the Dummy candidates to the bottom of the list (last name equals party code)
            if (c1.LastName == c1.Party.PartyCode)
            {
                return 1;
            }
            else if (c2.LastName == c2.Party.PartyCode)
            {
                return -1;
            }
            else
            {
                //Use number of votes from the previous election as our sort criteria
                if (c1.PreviousVotes != c2.PreviousVotes)
                {
                    if (c1.PreviousVotes < c2.PreviousVotes)
                    {
                        return 1;
                    }
                    if (c1.PreviousVotes > c2.PreviousVotes)
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
                    if (c1.Party.PartyPriority < c2.Party.PartyPriority)
                    {
                        return -1;
                    }
                    if (c1.Party.PartyPriority > c2.Party.PartyPriority)
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

    /// <summary>
    /// Helper class used to sort list of Candidates when there are results
    /// </summary>
    internal class CandidateComparerResults : IComparer<Candidate>
    {
        public int Compare(Candidate c1, Candidate c2)
        {
            //Use number of votes from the current election as our sort criteria
            if (c1.Votes != c2.Votes)
            {
                if (c1.Votes < c2.Votes)
                {
                    return 1;
                }
                if (c1.Votes > c2.Votes)
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
                if (c1.Party.PartyPriority < c2.Party.PartyPriority)
                {
                    return -1;
                }
                if (c1.Party.PartyPriority > c2.Party.PartyPriority)
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
