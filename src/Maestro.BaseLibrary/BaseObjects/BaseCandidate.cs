using Maestro.BaseLibrary.Interfaces;
using System;
using System.Data;
using System.Globalization;

namespace Maestro.BaseLibrary.BaseObjects
{
    public class BaseCandidate: IBaseCandidate
    {
        private int _candidateID;
        private BaseParty _party;
        private string _firstName;
        private string _lastName;
        private string _displayName1;
        private string _displayName2;
        private int _candidateType;
        private string _heroNote;
        private int _previousVotes;
        private string _gender;
        private string _candidateNote1;
        private string _candidateNote2;
        private bool _isRenegade;

        public int CandidateID { get { return _candidateID; } }
        public BaseParty Party { get { return _party; } }
        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }
        public int CandidateType { get { return _candidateType; } }
        public string HeroNote { get { return _heroNote; } }
        public int PreviousVotes { get { return _previousVotes; } }
        public string Gender { get { return _gender; } }
        public string CandidateNote1 { get { return _candidateNote1; } }
        public string CandidateNote2 { get { return _candidateNote2; } }
        public bool IsRenegade { get { return _isRenegade; } }

        /// <summary>
        /// Factory method to instantiate one BaseCandidate object from DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="party"></param>
        /// <returns></returns>
        public static BaseCandidate Create(DataRow row, BaseParty party)
        {
            BaseCandidate c = new BaseCandidate();
            try
            {
                //set the properties passed into the method
                c._candidateID = Int32.Parse(row["CandidateId"].ToString());
                c._party = party;
                c._firstName = row["FirstName"].ToString();
                c._lastName = row["LastName"].ToString();
                c._displayName1 = BaseUtilities.ProperCase(c._firstName + " " + c._lastName);
                c._displayName2 = c._displayName1;
                switch (row["Incumbent"].ToString())
                {
                    case "x":
                        //Incumbent candidate and incumbent party
                        c._candidateType = 3;
                        break;
                    case "w":
                        //Incumbent candidate
                        c._candidateType = 1;
                        break;
                    case "y":
                        //Incumbent party
                        c._candidateType = 2;
                        break;
                    default:
                        //no special status - just a regular candidate
                        c._candidateType = 0;
                        break;
                }
                c._heroNote = row["EnglishMediaNotes"].ToString();
                c._previousVotes = Int32.Parse(row["PreviousElectionVotes"].ToString());
                c._gender = row["Gender"].ToString();
                c._candidateNote1 = row["EnglishMediaNotes"].ToString();
                c._candidateNote2 = row["EnglishMediaNotes2"].ToString();
                c._isRenegade = (bool)row["IsMaestroRenegade"];
                return c;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Figure out which type of display name we have and assign to the appropriate property
        /// </summary>
        /// <param name="row"></param>
        public void UpdateDisplayName(DataRow row)
        {
            if (string.Compare(row["TypeName"].ToString(), "Candidates1", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName1 = row["DisplayName"].ToString();
            }
            if (string.Compare(row["TypeName"].ToString(), "Candidates2", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName2 = row["DisplayName"].ToString();
            }
        }
    }
}
