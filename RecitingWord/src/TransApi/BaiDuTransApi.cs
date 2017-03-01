using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class BaiDuTransApi
    {
        static BaiDuTransApi _Instance = new BaiDuTransApi("20161029000031016", "PjPHtTjSS1WziCB2AE3n");
        public static BaiDuTransApi Instance
        {
            get
            {
                return _Instance;
            }

        }
        private String appid;
        private String securityKey;
        private const String TRANS_API_HOST = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        public BaiDuTransApi(String appid, String securityKey)
        {
            this.appid = appid;
            this.securityKey = securityKey;
        }
        public List<string> GetTransResult(String query, String from, String to)
        {
            List<string> resule = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                string jsonResult = string.Empty;
                try
                {
                    jsonResult = Sever.HttpGet(TRANS_API_HOST, buildParams(query, from, to));
                    foreach (var item in JObject.Parse(jsonResult)["trans_result"]) resule.Add(item["dst"].Value<string>()); break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}:{1}", i, jsonResult);
                }
            }
            return resule;
        }

        private String buildParams(String query, String from, String to)
        {
            String salt = (DateTime.Now - DateTime.Parse("01/01/1970")).Ticks.ToString();// 随机数
            String src = appid + query + salt + securityKey; // 加密前的原文
            return $"q={query}&from={from}&to={to}&appid={appid}&salt={salt}&sign={MD5(src)}";
        }


        string MD5(string InputString)
        {
            //获取加密服务
            System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //获取要加密的字段，并转化为Byte[]数组
            byte[] InputEncrypt = System.Text.Encoding.UTF8.GetBytes(InputString);
            //加密Byte[]数组
            return md5CSP.ComputeHash(InputEncrypt).ToString("x");
        }
    }


    static public class Sever
    {
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?" + postDataStr));
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
    static public class ExMemose
    {
        public static string ToString(this byte[] Buffer, string formt)
        {
            StringBuilder HexString = new StringBuilder();
            foreach (var item in Buffer)
            {
                HexString.Append(item.ToString(formt));
            }
            return HexString.ToString();
        }
    }
}
