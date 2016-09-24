using CatchSongs.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CatchSongs
{

    public class CatchSongsMain
    {
        private int threadNumber = Common.ConfigReader.ThreadNumber;
        private int startId = Common.ConfigReader.StartId;
        private int endId = Common.ConfigReader.EndId;

        public static void Main(string[] args)
        {
            new CatchSongsMain().init();
        }

        public void init()
        {
            //DAL.GetSongList getsonglist = new DAL.GetSongList();

            //////catchSongs.Business.catchSongInfo catchsonginfo = new Business.catchSongInfo();

            //Thread threadGetAllSongId = new Thread(getsonglist.GetAllSongId);
            //threadGetAllSongId.Start();

            CatchSongInfo update = new CatchSongInfo();
            Thread threadUpdate = new Thread(update.QueryBySongIdAndUpdate);
            threadUpdate.Start();

            for (int threadIndex = 0; threadIndex < threadNumber; threadIndex++)
            {
                CatchSongInfo catchsonginfo = new CatchSongInfo();
                catchsonginfo.StartId = threadIndex + startId;
                catchsonginfo.EndId = endId - threadNumber + threadIndex;
                catchsonginfo.ThreadNumber = threadNumber;

                Thread threadCatch = new Thread(catchsonginfo.Run);
                //threadCatch.IsBackground = true;
                //Thread threadUpdate = new Thread(getsonglist.runMethod);
                //threadUpdate.IsBackground = true;

                //抓取歌曲信息线程  
                threadCatch.Start();

                //更新评论数线程
                //threadUpdate.Start();
            }
        }
    }
}
