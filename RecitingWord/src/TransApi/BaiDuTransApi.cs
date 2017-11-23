using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
                    //Console.WriteLine("{0}:{1}", i, jsonResult);
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
        public static string HttpPost(string Url, string postDataStr)
        {
            try
            {
                postDataStr = $@"[{{""text"":""{postDataStr}""}}]";

                var request = (HttpWebRequest)HttpWebRequest.Create(Url);
                request.Headers = new WebHeaderCollection();
                request.CookieContainer = BingCookie.Instance.cookie;
                //request.Accept = "application/json, text/javascript, */*; q=0.01";
                //request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                //request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
                //request.Headers.Add(HttpRequestHeader.Cookie, $@"MicrosoftApplicationsTelemetryDeviceId=4415bd06-9d65-b298-bbb3-30066a93d9ab; MicrosoftApplicationsTelemetryFirstLaunchTime=1498716824643; mtstkn={mtstkn}; srcLang=en; destLang=zh-CHS; smru_list=en; dmru_list=da%2Czh-CHS; sourceDia=en-US; destDia=zh-CN; _EDGE_V=1; MUIDB={MUIDB}; SRCHD=AF=NOFORM; SRCHUSR=DOB=20170602; MUID={MUID}; SRCHUID=V=2&GUID=2DB51B6BD0BE4DC89B0C9CBD02A86CB8; SRCHHPGUSR=CW=1600&CH=770&DPR=1&UTC=480; _EDGE_S=SID=0ED35BF1B54D6DA93F88514FB4EC6C95; srcLang=en; destLang=zh-CHS; smru_list=en; dmru_list=da%2Czh-CHS; sourceDia=en-US; destDia=zh-CN; _SS=SID=0ED35BF1B54D6DA93F88514FB4EC6C95; WLS=TS=63635505058");
                //request.Host = "www.bing.com";
                //request.Referer = "http://www.bing.com/translator/";
                request.ContentType = "application/json;charset=utf-8";
                request.ContentLength = Encoding.ASCII.GetByteCount(postDataStr);
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                writer.Write(postDataStr);
                writer.Flush();
                var response = (System.Net.HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
            }
            return "";
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

    class BingCookie
    {
        static BingCookie _Instance = new BingCookie();
        public static BingCookie Instance
        {
            get
            {
                return _Instance;
            }

        }
        private BingCookie()
        {
            var cookies = new CookieCollection();
            cookie = new CookieContainer();
            try
            {
                var Home = "http://www.bing.com/translator?mkt=zh-CN";
                var request = (HttpWebRequest)HttpWebRequest.Create(Home);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.CookieContainer = new CookieContainer();
                var response = (System.Net.HttpWebResponse)request.GetResponse();
                request.Abort();
                cookies = response.Cookies;

                foreach (Cookie item in cookies)
                {
                    cookie.Add(item);
                }
            }
            catch (Exception)
            {
            }
        }

        public CookieContainer cookie { get; }
    }
}
