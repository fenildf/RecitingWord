using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class BingTransApi
    {
        private const String TRANS_API_HOST = "http://xtk.azurewebsites.net/BingDictService.aspx";
        public static BingTrans getTransResult(string Word)
        {
            Word = Word.Trim();
            BingTrans Result = null;
            if ((Result = getDataBaseTransResult(Word)) != null)
            {
                //Console.WriteLine("DataBase : {0}", Word);
                return Result;
            }
            else
            {
                //Console.WriteLine("web : {0}", Word);
                //Result = getWebTransResult(Word);
                //if (Result.Error)
                {
                    Result = new BingTrans(Word, "", "", new List<defs>());
                    var BaiduResult = BaiduNewApi.Instance.GetTransResult(Word).ConfigureAwait(false).GetAwaiter().GetResult();
                    foreach (var item in BaiduResult.Kvs)
                    {
                        Result.defs.Add(new defs() { pos = item.v, def = item.v });
                        Console.WriteLine($"从百度返回 {item}");
                    }
                }
                TransResultToDataBase(Result);
                return Result;
            }
        }
        public static BingTrans getWebTransResult(string Word)
        {
            BingTrans Result = null;
            for (int i = 0; i < 10; i++)
            {
                string jsonResult = string.Empty;
                try
                {
                    //jsonResult = Sever.HttpGet(TRANS_API_HOST, "Word=" + Word);
                    jsonResult = Sever.HttpPost("http://www.bing.com/translator/api/Translate/TranslateArray?from=en&to=zh-CHS", Word);
                    
                    Result = BingTrans.BulidBingTrans(jsonResult);
                    Result.Word = Word;
                    break;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("getWebTransResult()->{0}:{2}:{1}", i, ex.Message, Word);
                    ErrorRecords(Word, string.Format("i = {2},jsonResult = {0},ex.Message = {1}", jsonResult, ex.Message, i));
                    if (Result == null) Result = new BingTrans(Word, string.Empty, string.Empty, new List<defs>()) { Error = true };
                }
            }
            return Result;
        }
        public static BingTrans getDataBaseTransResult(string Word)
        {
            try
            {
                var result = Mysql.Query($"select w.AmE,w.BrE,d.def,d.pos from word w,defs d where w.Word = d.Word and d.Word = \"{Word}\"", "").Tables[0].Rows;
                if (result.Count <= 0) return null;
                List<defs> defs = new List<defs>();
                foreach (DataRow item in result)
                {
                    defs.Add(new defs() { def = item.Field<string>("def"), pos = item.Field<string>("pos") });
                }
                return new BingTrans(Word, result[0].Field<string>("AmE"), result[0].Field<string>("Bre"), defs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取数据库失败：{Word}：{ex.Message}");
                ErrorRecords(Word, nameof(getDataBaseTransResult) + ex.Message);
                return null;
            }
        }
        public static void TransResultToDataBase(BingTrans bt)
        {
            if (bt != null)
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append($"insert into word(Word,AmE,BrE) values(\"{bt.Word}\",\"{bt.AmE}\",\"{bt.BrE}\");");
                    foreach (var item in bt.defs)
                    {
                        sql.Append($"insert into defs(Word, def, pos) values(\"{bt.Word}\",\"{item.def}\",\"{item.pos}\");");
                    }
                    Mysql.Insert(sql.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"导入数据库失败：{bt.Word}：{ex.Message}");
                    ErrorRecords(bt.Word, nameof(TransResultToDataBase) + ex.Message);
                }
            }
        }

        public static void ErrorRecords(string word, string messag)
        {
            try
            {
                Log.Write(string.Format("{0},{1}", word, messag));
                Mysql.Insert($"insert into error(Word,Message) values(\"{word}\",'');");
            }
            catch (Exception ex)
            {
                //Console.WriteLine("ErrorRecords()->{0}", ex.Message);
            }
        }
    }
    public class BingTrans
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Word">单词</param>
        /// <param name="AmE">美音</param>
        /// <param name="BrE">英音</param>
        /// <param name="defs">翻译</param>
        public BingTrans(string Word, string AmE, string BrE, List<defs> defs)
        {
            this.Word = Word; this.AmE = AmE; this.BrE = BrE; this.defs = defs;
        }
        public static BingTrans BulidBingTrans(string Json)
        {
            string Word = "";
            string Aem = "";
            string BrE = "";
            bool Exception = false;
            var defs = new List<defs>();
            try
            {
                var json = JObject.Parse(Json);

                foreach (var item in json["items"])
                {
                    defs.Add(new defs() { def = item.Value<string>("text"), pos = "" });
                }

                Exception = defs.Count == 0;

                //Word = json["word"].Value<string>();

                //try
                //{
                //    Aem = json["pronunciation"].Value<string>("AmE");
                //    BrE = json["pronunciation"].Value<string>("BrE");
                //}
                //catch (Exception) { }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("json解析错误->{0},message = {1}", Json, ex.Message);
                BingTransApi.ErrorRecords("BulidBingTransError", string.Format("json = {0},message = {1}", Json, ex.Message));
                Exception = true;
            }
            return new BingTrans(Word, Aem, BrE, defs) { Error = Exception };
        }
        public string Word;
        public string AmE;
        public string BrE;
        public List<defs> defs;

        public bool Error;
    }

    public class defs
    {
        /// <summary>
        /// 翻译结果
        /// </summary>
        public string def;
        /// <summary>
        /// 音标
        /// </summary>
        public string pos;
        public override string ToString()
        {
            return $"{pos} {def}";
        }
    }


    public static class Log
    {
        static FileStream LogStream = new FileStream("Log.log", FileMode.Append);

        public static void Write(string info, int skipFrames = 1)
        {
            StackTrace st = new StackTrace(new StackFrame(skipFrames, true));
            var infoData = Encoding.Default.GetBytes(string.Format("{0}--{1}---{2}\r\n", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ErrorLocation(st),
                info));
            LogStream.Write(infoData, 0, infoData.Length);
            LogStream.FlushAsync();
        }
        private static string ErrorLocation(StackTrace st)
        {
            StackFrame sf = st.GetFrame(0);
            return string.Format("{0}--{1}()--{2}Line--", Path.GetFileName(sf.GetFileName()), sf.GetMethod().Name, sf.GetFileLineNumber());
        }
    }
}
