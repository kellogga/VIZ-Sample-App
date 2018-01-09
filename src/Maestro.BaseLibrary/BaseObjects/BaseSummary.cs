using System;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Maestro.BaseLibrary.BaseObjects
{
    public class BaseSummary
    {
        private int _summaryID;
        private string _summaryName;
        private string _displayName1;
        private string _displayName2;
        public int SummaryID { get { return _summaryID; } }
        public string SummaryName { get { return _summaryName; } }
        public string DisplayName1 { get { return _displayName1; } }
        public string DisplayName2 { get { return _displayName2; } }

        /// <summary>
        /// Factory method to instantiate one BaseSummary object from DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static BaseSummary Create(DataRow row, int mediaId)
        {
            BaseSummary s = new BaseSummary();
            try
            {
                //set the properties passed into the method
                s._summaryID = Int32.Parse(row["Id"].ToString());
                if (mediaId == 1)
                {
                    s._summaryName = row["EnglishName"].ToString();
                }
                else
                {
                    if (row.Table.Columns.Contains("FrenchName"))
                    {
                        s._summaryName = row["FrenchName"].ToString();
                    }
                    else
                    {
                        s._summaryName = row["EnglishName"].ToString();
                    }
                }
                s._displayName1 = BaseUtilities.ProperCase(s._summaryName);
                s._displayName2 = s._displayName1;
                return s;
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
            if (string.Compare(row["TypeName"].ToString(), "Summaries1", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName1 = row["DisplayName"].ToString();
            }
            if (string.Compare(row["TypeName"].ToString(), "Summaries2", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
            {
                _displayName2 = row["DisplayName"].ToString();
            }
        }

        /// <summary>
        /// Override the ToString() method of this object to return the summary name, not the object type.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _summaryName.ToUpper();
        }

    }
}
