using Maestro.BaseLibrary.Interfaces;
using System;
using System.Data;
using System.Globalization;

namespace Maestro.BaseLibrary.BaseObjects
{
    public class BaseParty: IBaseParty
    {
        private int _partyID;
        private int _partyPriority;
        private string _partyCode;
        private string _partyName;
        private string _displayName1;
        private string _displayName2;

        public int PartyID { get { return _partyID; } }
        public int PartyPriority { get { return _partyPriority; } }
        public string PartyCode { get { return _partyCode; } }
        public string PartyName { get { return _partyName; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }

        /// <summary>
        /// Factory method to instantiate one BaseParty object from DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static BaseParty Create(DataRow row, int mediaId)
        {
            BaseParty b = new BaseParty();
            try
            {
                //set the properties passed into the method
                b._partyID = Int32.Parse(row["Id"].ToString());
                b._partyCode = row["EnglishCode"].ToString();
                if (mediaId == 1)
                {
                    b._partyName = row["EnglishName"].ToString();
                }
                else
                {
                    if (row.Table.Columns.Contains("FrenchName"))
                    {
                        b._partyName = row["FrenchName"].ToString();
                    }
                    else
                    {
                        b._partyName = row["EnglishName"].ToString();
                    }
                }
                b._displayName1 = BaseUtilities.ProperCase(b.PartyName);
                b._displayName2 = b._displayName1;
                b._partyPriority = Int32.Parse(row["sPriority"].ToString());
                return b;
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
            if (string.Compare(row["TypeName"].ToString(), "Parties1", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName1 = row["DisplayName"].ToString();
            }
            if (string.Compare(row["TypeName"].ToString(), "Parties2", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
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
            return _partyCode.ToUpper();
        }
    }
}
