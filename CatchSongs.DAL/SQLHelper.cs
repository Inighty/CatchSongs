/*****************************************************************************************************
 * 命名空间：CatchSongs.DAL
 * 类名称：SQLHelper
 * 文件名：SQLHelper
 * 创建年份：2016
 * 创建时间：2016/9/22 星期四 17:08:59
 * 创建人：孟超
 * 创建说明：
 *****************************************************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
*****************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using CatchSongs.Common;

namespace CatchSongs.DAL
{
    /// <summary>
    /// 数据库通用类
    /// </summary>
    public class SQLHelper
    {
        /// <summary>
        /// 数据库地址
        /// </summary>
        private static string mysqlAddress = ConfigReader.MysqlAddress;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLHelper()
        {

        }

        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>result</returns>
        public object QueryResult(string sql)
        {
            //"SELECT count(*) FROM music_v2"
            using (MySqlConnection conn = new MySqlConnection(mysqlAddress))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 有则更新，无则添加
        /// </summary>
        /// <param name="songId">songId</param>
        /// <param name="songName">songName</param>
        /// <param name="artist">artist</param>
        /// <param name="count">count</param>
        public void SolveSongs(int songId, string songName, string artist, int count)
        {
            StringBuilder updateCmd = new StringBuilder();
            //判断songId是否已经存在
            DAL.GetSongList getsonglist = new DAL.GetSongList();
            //List<int> list = getsonglist.getAllSongs();
            //有则更新，无则添加
            //if (list.Contains(songId))
            //{
            //    getsonglist.UpdateCount(songId.ToString(), count.ToString());
            //}
            //else
            //{
            MySqlConnection conn = null;
            try
            {
                using (conn = new MySqlConnection(mysqlAddress))
                {
                    //歌名可能会有',导致插入失败，转为双引号
                    updateCmd.AppendFormat("insert into music_v2 (songId, songName, artist, count) values ({0},'{1}','{2}',{3}) on duplicate key update count = {4}", songId, songName.Replace("'", "''"), artist, count, count);
                    string updateCmdstr = updateCmd.ToString();
                    updateCmd.Clear();
                    //private string updateCmd = "update music_v2 set count ";
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Console.WriteLine("命令：" + updateCmdstr + "\n");

                    using (MySqlCommand cmd = new MySqlCommand(updateCmdstr, conn))
                    {
                        //执行sql
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                //出现主键冲突代表已存在  则更新
                if (e.ToString().ToUpper().Contains("Duplicate entry".ToUpper()))
                {
                    UpdateCount(songId, count);
                }
                else
                //else if (e.ToString().ToUpper().Contains("syntax".ToUpper()))
                {
                    Console.WriteLine(e.ToString() + "\n" + updateCmd);
                }
            }
        }
      

        /// <summary>
        /// 更新评论数
        /// </summary>
        /// <param name="songId">songId</param>
        /// <param name="count">count</param>
        public void UpdateCount(int songId, int count)
        {
            StringBuilder updateCmd = new StringBuilder();
            MySqlConnection conn = null;
            try
            {
                using (conn = new MySqlConnection(mysqlAddress))
                {
                    //StringBuilder updateCmd = new StringBuilder();
                    updateCmd.AppendFormat("update music_v2 set count = {0} where songId = {1}", count, songId);
                    string updateCmdstr = updateCmd.ToString();
                    //private string updateCmd = "update music_v2 set count ";
                    using (MySqlCommand cmd = new MySqlCommand(updateCmdstr, conn))
                    {

                        conn.Open();
                        //执行sql
                        cmd.ExecuteNonQuery();
                        updateCmd.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                TxtLog txtlog = new TxtLog();
                txtlog.log(e.ToString() + "\n" + updateCmd + "\n", 0);
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            //MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            //adp.SelectCommand = cmd;
            //DataSet ds = new DataSet();
            ////填充dataset
            //adp.Fill(ds);
        }

    }
}
