using System;
using System.Configuration;
using VizWrapper;

namespace Maestro.BaseLibrary.Config
{
    public class Scene : ConfigurationElement
    {

        #region "Properties"
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return Convert.ToString(this["Name"]).ToLower(); }
        }

        [ConfigurationProperty("InternalName", IsRequired = true)]
        public string InternalName
        {
            get { return Convert.ToString(this["InternalName"]); }
        }

        [ConfigurationProperty("Channel", DefaultValue = 0, IsRequired = false)]
        public int Channel
        {
            get { return Convert.ToInt32(this["Channel"]); }
        }

        public string ChannelName
        {
            get
            {
                switch (Convert.ToInt32(this["Channel"]))
                {
                    case 0:
                        return "*FRONT_LAYER";
                    case 1:
                        //"*MAIN_LAYER"
                        return "";
                    case 2:
                        return "*BACK_LAYER";
                    default:
                        return "";
                }
            }
        }

        [ConfigurationProperty("DatapoolName", IsRequired = true)]
        public string DatapoolName
        {
            get { return Convert.ToString(this["DatapoolName"]); }
        }

        [ConfigurationProperty("NumberOfItems", DefaultValue = 0, IsRequired = false)]
        public int NumberOfItems
        {
            get { return Convert.ToInt32(this["NumberOfItems"]); }
        }

        [ConfigurationProperty("PauseInterval", IsRequired = true)]
        public int PauseInterval
        {
            get { return Convert.ToInt32(this["PauseInterval"]); }
        }

        [ConfigurationProperty("TypeOfData", IsRequired = true)]
        public DataType TypeOfData
        {
            get
            {
                //Translate the string from the config file into an enum
                DataType dataType = DataType.None;
                switch (this["TypeOfData"].ToString())
                {
                    case "Region":
                        dataType = DataType.Region;
                        break;
                    case "Riding":
                        dataType = DataType.Riding;
                        break;
                    case "RegionRiding":
                        dataType = DataType.RegionRiding;
                        break;
                    case "RegionParty":
                        dataType = DataType.RegionParty;
                        break;
                    case "None":
                        dataType = DataType.None;
                        break;
                    case "Summary":
                        dataType = DataType.Summary;
                        break;
                }
                return dataType;
            }
        }

        [ConfigurationProperty("ShowOthers", DefaultValue = false, IsRequired = false)]
        public bool ShowOthers
        {
            get { return Convert.ToBoolean(this["ShowOthers"]); }
            set { this["ShowOthers"] = value; }
        }


        [ConfigurationProperty("CalculateVoteShare", DefaultValue = false, IsRequired = false)]
        public bool CalculateVoteShare
        {
            get { return Convert.ToBoolean(this["CalculateVoteShare"]); }
        }

        #endregion

    }
}

