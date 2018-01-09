using System;
using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class MaestroConfig : ConfigurationSection
    {
        [ConfigurationProperty("Scenes", IsRequired = true)]
        public Scenes Scenes
        {
            get { return (Scenes)this["Scenes"]; }
        }

        [ConfigurationProperty("VIZConfigs", IsRequired = true)]
        public VIZConfigs VIZConfigs
        {
            get { return (VIZConfigs)this["VIZConfigs"]; }
        }

        [ConfigurationProperty("MiniMessages", IsRequired = true)]
        public MiniMessages MiniMessages
        {
            get { return (MiniMessages)this["MiniMessages"]; }
            set { this["MiniMessages"] = value; }
        }

        [ConfigurationProperty("WomanSeats", IsRequired = true)]
        public WomanSeats WomanSeats
        {
            get { return (WomanSeats)this["WomanSeats"]; }
            set { this["WomanSeats"] = value; }
        }

        public static MaestroConfig GetConfig()
        {
            try
            {
                return (MaestroConfig)ConfigHelper.GetSection("MaestroConfig", "MaestroConfigSettings");
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading config file", ex);
            }
        }

    }
}
