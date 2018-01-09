using System;
using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class WomanSeat : ConfigurationElement
    {
        [ConfigurationProperty("PartyCode", IsRequired = true)]
        public string PartyCode
        {
            get { return Convert.ToString(this["PartyCode"]); }
            set { this["PartyCode"] = value; }
        }

        [ConfigurationProperty("SeatCount", IsRequired = true)]
        public string SeatCount
        {
            get { return Convert.ToString(this["SeatCount"]); }
            set { this["SeatCount"] = value; }
        }
    }
}
