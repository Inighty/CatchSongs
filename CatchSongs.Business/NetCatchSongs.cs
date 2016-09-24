using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace CatchSongs
{
    public class NetCatchSongs
    {

        //private static string info_url = "http://music.163.com/api/song/detail/?ids=%5B#songIds#%5D";
        //private static string params1 = "MjdcjJ0FbCPJR90VfRep5nnE4NMItX4f3xjdJPlbvatfoUbt6g4RBELo7JcOqRU176SC343L%2BLsFPRwEVdVFrlJf0tE2ATMx%2FtnCNjbdgroyG4OQO4GblXM51c5%2F45yoDigdNTiejTFu%2Fq7wOffZ1NhKJmWbfBlPDPpJ%2B07ijVergSaH4cq6QKrpSivU7Qp7&encSecKey=6ba5031c2f2d3559a45787c7d4b8dada1c154de42576a5ae137e7e5c04096c303cfb6d19a9fe46391906007cdc714c9a7043e76d2bef9320a2a3494b3b86a5880261480486400e717cd1f71141db78e0fb39727c5b30b80fb6f62a1b03bccc59923207f125f1fa4edd07172733e14480d68c3ecdee9613235e9c30bc5f712b11";


        public void runCatch(string songlist)
        {
            string comment_url = "http://music.163.com/weapi/v1/resource/comments/R_SO_4_#songId#/?csrf_token=";
            comment_url = comment_url.Replace("#songId#", songlist);
            DAL.Send posturl = new DAL.Send();
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("params", "ea9WCu%2BTyFQ41I178iV0doeMVmKu6U7WyVdqipaWyrHB%2BbNn95rNVgvP3GqiMIHm");
            //dic.Add("encSecKey", "21062e4d163cdaf19e78f8ce418fafcd1da478c54d9fb1a4e2c4c59cedd743c2f3ade7d74cdf67e7a632372fd449ee6366753f71cbad5146fdb481fc4f876ac53040a268ab80ef6ca7e15d560b91cc40743a837fcbbbf312fdda610cf7ae49ccfd873ceabc57f27ff234648f4ec084457e730448b98da435931168a1d3fcae4a");
            DAL.GetSongList getsonglist = new DAL.GetSongList();
            //post 获取评论返回数据
            string commentStr = posturl.postdata(comment_url,songlist);
            JObject jobj = JObject.Parse(commentStr);
            //从对象中获取total的值
            string needStr = jobj["total"].Value<string>();

            getsonglist.UpdateCount(int.Parse(songlist), int.Parse(needStr));
        }
    }
}