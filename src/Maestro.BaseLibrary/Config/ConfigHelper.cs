using System;
using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class ConfigHelper
    {
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static ConfigurationSectionGroup GetConfigGroup(string groupName)
        {
            ConfigurationSectionGroup csg = new ConfigurationSectionGroup();
            try
            {
                csg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup(groupName);
            }
            catch (Exception)
            {
                csg = (ConfigurationSectionGroup)ConfigurationManager.GetSection(groupName);
            }
            return csg;
        }

        public static ConfigurationSection GetSection(string sectionName)
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(sectionName);
        }

        public static ConfigurationSection GetSection(string sectionName, string groupName)
        {
            return GetConfigGroup(groupName).Sections.Get(sectionName);
        }

        public static void WriteToAppSettings(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }

    }
}
