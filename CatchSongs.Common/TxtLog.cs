﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CatchSongs.Common
{
    public class TxtLog
    {
        //public static ReaderWriterLock readerWriterLock = new ReaderWriterLock();
        private static string path = ConfigReader.LogPath + @"\log\";
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <param name="i">i = 0 异常日志 i = 1 记录各个线程日志 i = 2 记录更新歌曲日志</param>
        public void log(string ex, int i)
        {
            string file = string.Empty;
            
            if (i == 0)
            {
                file = path + "error.txt";
            }
            else if (i == 1 && ConfigReader.ThreadLogSwitch == "开")
            {
                string threadPath = path + @"threadLog\";
                file = threadPath + Thread.CurrentThread.ManagedThreadId.ToString() + ".txt";
            }
            else if (i == 2)
            {
                file = path + "UpdateSongsLog.txt";
            }
            else {
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            byte[] myByte = Encoding.UTF8.GetBytes(ex + "\r\n");
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Position = fs.Length;
                //将待写入内容追加到文件末尾  
                fs.Write(myByte, 0, myByte.Length);
                fs.Dispose();
            }
        }
    }
}

