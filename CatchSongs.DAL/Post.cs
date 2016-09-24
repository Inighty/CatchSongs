using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace catchSongs.DAL
{
    public class Post
    {
        //传递的参数
        private static string postString = "params=MjdcjJ0FbCPJR90VfRep5nnE4NMItX4f3xjdJPlbvatfoUbt6g4RBELo7JcOqRU176SC343L%2BLsFPRwEVdVFrlJf0tE2ATMx%2FtnCNjbdgroyG4OQO4GblXM51c5%2F45yoDigdNTiejTFu%2Fq7wOffZ1NhKJmWbfBlPDPpJ%2B07ijVergSaH4cq6QKrpSivU7Qp7&encSecKey=6ba5031c2f2d3559a45787c7d4b8dada1c154de42576a5ae137e7e5c04096c303cfb6d19a9fe46391906007cdc714c9a7043e76d2bef9320a2a3494b3b86a5880261480486400e717cd1f71141db78e0fb39727c5b30b80fb6f62a1b03bccc59923207f125f1fa4edd07172733e14480d68c3ecdee9613235e9c30bc5f712b11";
        public string postdata(string url,string songId,Boolean flag)
        {
            //编码
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            //地址
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Referer", "http://music.163.com/song?id=" + songId);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("MediaType", "APPLICATION_FORM_URLENCODED_TYPE");
            //得到返回字符流  

            if (flag)
            {

                byte[] responseData = webClient.UploadData(url, "POST", postData);
                webClient.Dispose();
                //解码 
                string srcString = Encoding.UTF8.GetString(responseData);
                return srcString;
            }

            else {
                byte[] responseData = webClient.UploadData(url, "POST", postData);
                webClient.Dispose();
                //解码 
                string srcString = Encoding.UTF8.GetString(responseData);
                return srcString;
                
            }
            
                //获取到的json转为对象
                //JObject jobj = JObject.Parse(srcString);
                ////从对象中获取total的值
                //string needStr = jobj["total"].Value<string>();
                //return needStr;
            
        }

    }
}