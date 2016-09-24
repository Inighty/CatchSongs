using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using CatchSongs;

namespace CatchSongs.DAL
{

    public class GetSongList
    {
        public static ReaderWriterLock readerWriterLock = new ReaderWriterLock();
        public static List<int> list = null;
        StringBuilder updateCmd = new StringBuilder();
        private static string mysqlAddress = Common.ConfigReader.MysqlAddress;
        Common.TxtLog txtlog = new Common.TxtLog();

        public void GetAllSongId()
        {
            while (true)
            {
                readerWriterLock.AcquireWriterLock(1000000000);
                ////List<int> list = new List<int>();
                MySqlConnection conn = new MySqlConnection(mysqlAddress);
                MySqlCommand cmd = new MySqlCommand("SELECT songId FROM music_v2", conn);
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    //执行sql
                    if (list != null)
                    {
                        list.Clear();
                    }
                    else
                    {
                        list = new List<int>();
                    }
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        int colCount = row.ItemArray.Count();
                        int items = 0;
                        items = Convert.ToInt32(row.ItemArray[0]);
                        list.Add(items);
                    }
                    txtlog.log(DateTime.Now.ToString()+" 获取到"+list.Count().ToString()+"条数据",2);
                }
                catch (Exception e)
                {
                    txtlog.log(e.ToString(),0);
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
                readerWriterLock.ReleaseWriterLock();
                //每过配置参数 时间 更新一次list
                Thread.Sleep(1000 * 60 * Common.ConfigReader.GetAllSongIdInterval);
            }
        }
        //存放要抓的list 和 string 这里只能一条一条取 废弃
        //List<int> list100 = new List<int>();
        //StringBuilder toUpdateSongs = new StringBuilder();

        //int k = 0;
        //int j = 1;
        //while (true)
        //{

        ////记录上一次取了这么多个，下次接着继续取
        //StringBuilder toUpdateSongs = new StringBuilder();
        //for (int i = k; i < j + k; i++)
        //{

        //    list100.Add(list[i][0]);
        //    toUpdateSongs.AppendFormat("{0},", list100[i]);
        //}
        //Console.WriteLine(toUpdateSongs.Remove(toUpdateSongs.Length - 1, 1) + "\n");
        ////toUpdateSongs.Clear();
        //k += 1;
        //if (k > list.Count - 1)
        //{
        //    break;
        //}

        //得到了要抓的list   list100  转为string

        //for (int i = 0; i < list.Count; i++)
        //{
        //    Console.WriteLine(list[i][0] + "\n");
        //}

        //public void runMethod()
        //{
        //    while (true)
        //    {
        //        readerWriterLock.AcquireReaderLock(1000000000);
        //        if (list != null)
        //        {
        //            catchCommet(list);
        //        }

        //        readerWriterLock.ReleaseReaderLock();
        //        Thread.Sleep(5000);
        //    }
        //}


        ////开始抓取评论
        //public void catchCommet(List<int> list)
        //{
        //    //死循环  让它一直抓

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        //对100条数据进行抓取
        //        NetCatchSongs netcatchsongs = new NetCatchSongs();
        //        netcatchsongs.runCatch(list[i].ToString());
        //    }

        //}

        //处理获取到的信息
        public void SolveSongs(int songId, string songName, string artist, int count)
        {

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
                    
                    MySqlCommand cmd = new MySqlCommand(updateCmdstr, conn);
                    //执行sql
                    cmd.ExecuteNonQuery();

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
        //}

        //更新评论数
        public void UpdateCount(int songId, int count)
        {
            MySqlConnection conn = null;
            try
            {
                using (conn = new MySqlConnection(mysqlAddress))
                    //StringBuilder updateCmd = new StringBuilder();
                    updateCmd.AppendFormat("update music_v2 set count = {0} where songId = {1}", count, songId);
                string updateCmdstr = updateCmd.ToString();
                //private string updateCmd = "update music_v2 set count ";


                MySqlCommand cmd = new MySqlCommand(updateCmdstr, conn);

                conn.Open();
                //执行sql
                cmd.ExecuteNonQuery();
                updateCmd.Clear();
            }
            catch (Exception e)
            {
                txtlog.log(e.ToString() + "\n" + updateCmd + "\n",0);
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
