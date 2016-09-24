using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace CatchSongs.Business
{
    public class CatchSongInfo
    {
        /// <summary>
        /// 信息URL
        /// </summary>
        private static string info_url = "http://music.163.com/api/song/detail/?ids=%5B#songIds#%5D";

        /// <summary>
        /// 每次抓取歌曲数目
        /// </summary>
        private int get_info_once = Common.ConfigReader.Get_Info_Once;

        /// <summary>
        /// 最小评论数
        /// </summary>
        private int MinCommentCount = Common.ConfigReader.MinCommentCount;

        Common.TxtLog txtlog = new Common.TxtLog();


        ////封装，传递多个参数
        private int startId;

        /// <summary>
        /// 终止ID
        /// </summary>
        private int endId;

        /// <summary>
        /// 线程数
        /// </summary>
        private int threadNumber;

        /// <summary>
        /// 起始ID
        /// </summary>
        public int StartId
        {
            get { return this.startId; }
            set { this.startId = value; }
        }


        /// <summary>
        /// 终止ID
        /// </summary>
        public int EndId
        {
            get { return this.endId; }
            set { this.endId = value; }
        }



        /// <summary>
        /// 线程数
        /// </summary>
        public int ThreadNumber
        {
            get { return this.threadNumber; }
            set { this.threadNumber = value; }
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Run()
        {
            this.GetSongs(this.startId, this.endId, this.threadNumber);
        }

        /// <summary>
        /// 抓取
        /// </summary>
        /// <param name="startId">起始ID</param>
        /// <param name="endId">终止ID</param>
        /// <param name="threadNumber">线程数</param>
        private void GetSongs(int startId, int endId, int threadNumber)
        {
            DAL.Send post = new DAL.Send();
            DAL.GetSongList getsonglist = new DAL.GetSongList();
            StringBuilder onceGet = new StringBuilder();
            StringBuilder artists = new StringBuilder();

            for (int songId = startId; songId <= endId; )
            {
                ////List<int> list = getsonglist.getAllSongs();
                try
                {
                    for (int index = 0; index < this.get_info_once; index++)
                    {
                        onceGet.Append((songId + index * threadNumber <= endId) ? (songId + index * threadNumber).ToString() : string.Empty).Append((songId + index * threadNumber <= endId) ? "," : string.Empty);
                    }

                    songId += this.get_info_once * this.threadNumber;
                    if (songId > endId)
                    {
                        songId = endId;
                    }
                    string onceGetstr = onceGet.ToString().Substring(0, onceGet.ToString().Length - 1);
                    ////txtlog.writeLog(Thread.CurrentThread.ManagedThreadId.ToString()+" 获取这个songId: "+onceGetstr);
                    onceGet.Clear();
                    string toGet = info_url.Replace("#songIds#", onceGetstr);

                    ////post 获取歌曲信息
                    string songInfoJson = post.getdata(toGet);

                    ////获取到的json转为对象
                    JObject jobj = JObject.Parse(songInfoJson);

                    for (int i = 0; i < jobj["songs"].Count(); i++)
                    {
                        ////不用全局  用局部变量 用完后需要初始化
                        string comment_url = "http://music.163.com/weapi/v1/resource/comments/R_SO_4_#songId#/?csrf_token=";
                        int exsitSongId = Convert.ToInt32(jobj["songs"][i]["id"].ToString());

                        ////从对象中获取songName
                        string songName = jobj["songs"][i]["name"].ToString();

                        ////从对象中获取artist

                        for (int j = 0; j < jobj["songs"][i]["artists"].Count(); j++)
                        {
                            artists.AppendFormat("{0},", jobj["songs"][i]["artists"][j]["name"].ToString());
                        }
                        string artist = artists.ToString().Substring(0, artists.Length - 1);
                        artists.Clear();
                        ////好像对这些字段做了处理 先不用了
                        //////获取bMusic
                        //string bMusic = jobj["songs"][i]["bMusic"].ToString();
                        //string hMusic = jobj["songs"][i]["hMusic"].ToString();
                        //string lMusic = jobj["songs"][i]["lMusic"].ToString();
                        //string mMusic = jobj["songs"][i]["mMusic"].ToString();

                        //if (string.IsNullOrEmpty(bMusic) && string.IsNullOrEmpty(hMusic) && string.IsNullOrEmpty(lMusic) && string.IsNullOrEmpty(mMusic))
                        //{ return; }
                        //else if (!(string.IsNullOrEmpty(bMusic) || string.IsNullOrEmpty(hMusic) || string.IsNullOrEmpty(lMusic) || string.IsNullOrEmpty(mMusic)))
                        //{
                        //    if (jobj["songs"][i]["bMusic"]["dfsId"].ToString() == "0" &&
                        //        jobj["songs"][i]["hMusic"]["dfsId"].ToString() == "0" &&
                        //        jobj["songs"][i]["lMusic"]["dfsId"].ToString() == "0" &&
                        //        jobj["songs"][i]["mMusic"]["dfsId"].ToString() == "0")
                        //    { return; }
                        //}
                        //else
                        //{
                        //    if (!string.IsNullOrEmpty(bMusic))
                        //    {
                        //        if (jobj["songs"][i]["bMusic"]["dfsId"].ToString() == "0")
                        //        { return; }
                        //    }

                        //    if (!string.IsNullOrEmpty(hMusic))
                        //    {
                        //        if (jobj["songs"][i]["hMusic"]["dfsId"].ToString() == "0")
                        //        { return; }
                        //    }

                        //    if (!string.IsNullOrEmpty(lMusic))
                        //    {
                        //        if (jobj["songs"][i]["lMusic"]["dfsId"].ToString() == "0")
                        //        { return; }
                        //    }

                        //    if (!string.IsNullOrEmpty(mMusic))
                        //    {
                        //        if (jobj["songs"][i]["mMusic"]["dfsId"].ToString() == "0")
                        //        { return; }
                        //    }
                        //}
                        ////post 获取评论数
                        comment_url = comment_url.Replace("#songId#", exsitSongId.ToString());
                        string songCommentJson = post.postdata(comment_url, exsitSongId.ToString());

                        JObject jobj_comment = JObject.Parse(songCommentJson);
                        ////从对象中获取total的值
                        string needStr = jobj_comment["total"].Value<string>();

                        int count = Convert.ToInt32(needStr);
                        ////获取的评论数大于等于最小值，就进行处理

                        if (count >= this.MinCommentCount)
                        {
                            getsonglist.SolveSongs(exsitSongId, songName, artist, count);
                        }
                        this.txtlog.log(DateTime.Now.ToString() + " 这个点我没偷懒哦！", 1);
                        Thread.Sleep(200);
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString() + "\n");
                    ////common.txtLog txtlog = new common.txtLog();
                    this.txtlog.log(e.ToString() + "\n", 0);
                }
            }
        }
    }
}