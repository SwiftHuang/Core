using System;
using System.Configuration;
using System.Collections.Generic;

namespace hwj.CommonLibrary.Object
{
    public static class ConfigHelper
    {
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
        public static Dictionary<string, string> GetConnectionStrings(string[] keys)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (string k in keys)
                ret[k] = GetConnectionString(k);
            return ret;
        }
        public static string GetAppSetting(string key)
        {
            return GetAppSetting(key, "");
        }
        public static string GetAppSetting(string key, string defaultValue)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                return ConfigurationManager.AppSettings[key];
            else
                return defaultValue;
        }
        public static Dictionary<string, string> GetAppSettings(string[] keys)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (string k in keys)
                ret[k] = GetAppSetting(k);
            return ret;
        }
        //public static int GetIntergerSetting(string key)
        //{
        //    return NumberHelper.ToInt(GetAppSetting(key), 0);
        //}
        public static bool GetBoolSetting(string key)
        {
            return GetAppSetting(key) == "1";
        }
    }
}
