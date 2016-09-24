using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchSongs.Common
{
    public class ConfigReader
    {

        /// <summary>
        /// 配置参数字典
        /// </summary>
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        public static int StartId
        {
            get { return GetConfigValue("StartId", 0); }
        }

        public static int EndId
        {
            get { return GetConfigValue("EndId", 410000000); }
        }

        public static int ThreadNumber
        {
            get { return GetConfigValue("ThreadNumber", 1); }
        }

        public static int Get_Info_Once
        {
            get { return GetConfigValue("Get_Info_Once", 1); }
        }

        public static int MinCommentCount
        {
            get { return GetConfigValue("MinCommentCount", 0); }
        }
        /// <summary>
        /// 更新评论数大于该值的歌曲
        /// </summary>
        public static int UpdateSongsMinCount
        {
            get { return GetConfigValue("UpdateSongsMinCount", 0); }
        }

        public static string MysqlAddress
        {
            get { return GetConfigValue("MysqlAddress", "Data Source=192.168.1.70;Port=3306;Database=test;User ID=writeuser;Password=writeuser;Charset=utf8"); }
        }

        public static int GetAllSongIdInterval
        {
            get { return GetConfigValue("GetAllSongIdInterval", 5); }
        }

        public static string LogPath
        {
            get { return GetConfigValue("LogPath", Environment.CurrentDirectory.ToString()); }
        }

        public static string ThreadLogSwitch
        {
            get { return GetConfigValue("ThreadLogSwitch", "关"); }
        }














        private static T GetConfigValue<T>(string name, T defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(config.ContainsKey(name) ? config[name] : string.Empty))
                {
                    //T value = (T)Convert.ChangeType(config[name], typeof(T));
                    config[name] = ConfigurationManager.AppSettings[name];
                }
                T value = (T)Convert.ChangeType(config[name], typeof(T));
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue;
                }
                else {
                    return value;
                }

            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
