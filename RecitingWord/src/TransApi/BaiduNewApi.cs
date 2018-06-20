using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    public class BaiduNewApi
    {
        static BaiduNewApi _Instance = new BaiduNewApi();
        public static BaiduNewApi Instance
        {
            get
            {
                return _Instance;
            }

        }
        private BaiduNewApi()
        {
            var HttpClientHandler = new HttpClientHandler();
            HttpClientHandler.CookieContainer = BingCookie.Instance.cookie;
            hc = new HttpClient(HttpClientHandler);
            //BingCookie.Instance.cookie

            //Task.Run(async ()=> {
            //    var result = GetTransResult("translate");
            //});
        }


        public async Task<BaiduTransStruct> GetTransResult(string word)
        {
            var TransResult = new BaiduTransStruct();

            var result = UnicodeToGB(await Post("http://fanyi.baidu.com/sug", $"kw={word}"));
            var JO = JObject.Parse(result)["data"];
            foreach (var item in JO)
            {
                TransResult.Kvs.Add(new kv() { k = item["k"].ToString(), v = item["v"].ToString() });
            }
            return TransResult;
        }





        public HttpClient hc { get; }
        public async Task<string> Post(string DestUri, string Parame)
        {
            using (var sc = new StringContent(Parame))
            {
                #region MyRegion
                //hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("Application/json"));
                //hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/javascript"));
                //hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.01));
                //hc.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                //hc.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "537.36"));
                //hc.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "62.0.3202.94"));
                //hc.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Safari", "537.36"));

                //hc.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                //hc.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                //hc.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-CN"));
                //hc.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh"));

                //hc.DefaultRequestHeaders.Host = "fanyi.baidu.com";
                //hc.DefaultRequestHeaders.Connection.Add("keep-alive");

                //hc.DefaultRequestHeaders.Add("Origin", "http://fanyi.baidu.com");
                //hc.DefaultRequestHeaders.Add("X-Requested-Wit", "XMLHttpRequest");
                //hc.DefaultRequestHeaders.Add("Referer", "http://fanyi.baidu.com/translate"); 
                #endregion
                sc.Headers.ContentLength = Parame.Length;
                sc.Headers.ContentType.CharSet = "UTF-8";//重要
                sc.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";//重要

                using (var htm = await hc.PostAsync(DestUri, sc))
                {
                    return await htm.Content.ReadAsStringAsync();
                }
            }
        }

        public string UnicodeToGB(string Soure)
        {
            if (string.IsNullOrWhiteSpace(Soure)) return string.Empty;
            var New = new StringBuilder(Soure.Length);

            var Word = string.Empty;
            ushort intger = 0;
            foreach (var item in Soure.Split(new string[] { @"\u" }, StringSplitOptions.RemoveEmptyEntries))
            {

                if (item.Length == 4)
                {
                    if (ushort.TryParse(item, NumberStyles.HexNumber, null, out intger))
                    {
                        New.Append((char)intger);
                        continue;
                    }
                }
                else if (item.Length > 4)
                {
                    if (ushort.TryParse(item.Substring(0, 4), NumberStyles.HexNumber, null, out intger))
                    {
                        New.Append((char)intger);
                        New.Append(item.Substring(4, item.Length - 4));
                        continue;
                    }
                }

                New.Append(item);
            }

            return New.ToString();

        }
    }

    public class BaiduTransStruct
    {
        public BaiduTransStruct()
        {
            Kvs = new List<kv>();
        }
        public List<kv> Kvs;
        public override string ToString()
        {
            return $"Count = {Kvs.Count},{string.Join(",",Kvs)}";
        }
    }
    public class kv
    {
        public string k;
        public string v;
        public override string ToString()
        {
            return $"{k}:{v}";
        }
    }

}
