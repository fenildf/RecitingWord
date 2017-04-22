using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MVVM;

namespace RecitingWord
{
    class GoogleTransApi
    {
        public static GoogleTransApi Instance { get; } = new GoogleTransApi();

        public BingTrans Trans(string word, string fromLanguage, string toLanguage)
        {
            var tk = JavaScriptHandle.Instance.tk(word, "414398.1781904367");
            string TransUrl = $"https://translate.google.com/translate_a/single?client=t&sl=auto&tl={toLanguage}&hl=en&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=1&pc=1&ssel=0&tsel=0&kc=2&tk={tk}&q={word}";
            var TransResultJson = string.Empty;
            try
            {
                var googleTransUrl = new StringBuilder();
                TransResultJson = Sever.HttpGet(TransUrl, "");

                string Aem = "";
                string BrE = "";
                var defs = new List<defs>();
                try
                {
                    var jarray = JArray.Parse(TransResultJson);
                    if (jarray[1].Count() > 0)
                    {
                        foreach (var Item in jarray[1])
                        {
                            defs.Add(new RecitingWord.defs()
                            {
                                def = Item[1].GetEnumeratorString(),
                                pos = Item[0].ToString()
                            });
                        }
                    }
                    else
                    {
                        defs.Add(new RecitingWord.defs()
                        {
                            def = jarray[0][0][0].ToString(),
                            pos = string.Empty
                        });

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("json解析错误->{0},message = {1}", TransResultJson, ex.Message);
                    BingTransApi.ErrorRecords("BulidBingTransError", string.Format("json = {0},message = {1}", TransResultJson, ex.Message));
                }

                return new BingTrans(word, Aem, BrE, defs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("服务器错误 -> {0}", ex.Message);
            }
            return new BingTrans("", "", "", new List<defs>());
        }
        public BingTrans SentenceTrans(string Sentence, string fromLanguage, string toLanguage)
        {
            var tk = JavaScriptHandle.Instance.tk(Sentence, "414398.1781904367");
            string TransUrl = $"https://translate.google.com/translate_a/single?client=t&sl=auto&tl={toLanguage}&hl=en&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=1&pc=1&ssel=0&tsel=0&kc=2&tk={tk}&q={Sentence}";
            var TransResultJson = string.Empty;
            try
            {
                var googleTransUrl = new StringBuilder();
                TransResultJson = Sever.HttpGet(TransUrl, "");

                string Aem = "";
                string BrE = "";
                var defs = new List<defs>();
                try
                {
                    var jarray = JArray.Parse(TransResultJson);

                    foreach (var Item in jarray[5][0][2])
                    {
                        defs.Add(new RecitingWord.defs()
                        {
                            def = Item[0].ToString(),
                            pos = string.Empty
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("json解析错误->{0},message = {1}", TransResultJson, ex.Message);
                    BingTransApi.ErrorRecords("BulidBingTransError", string.Format("json = {0},message = {1}", TransResultJson, ex.Message));
                }

                return new BingTrans(Sentence, Aem, BrE, defs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("服务器错误 -> {0}", ex.Message);
            }
            return new BingTrans("", "", "", new List<defs>());
        }

        public BingTrans getTransResult(string Word)
        {

            Word = Word.Trim();
            BingTrans Result = null;
            if ((Result = BingTransApi.getDataBaseTransResult(Word)) != null)
            {
                Console.WriteLine($"DataBase : {Word} -> {string.Join(" ", Result.defs)}");
                return Result;
            }
            else
            {
                Result = Trans(Word, "en", "zh-CN");
                BingTransApi.TransResultToDataBase(Result);
                Console.WriteLine($"web : {Word} -> {string.Join(" ", Result.defs)}");
                return Result;
            }
        }
        public BingTrans getSentenceTransResult(string Sentence)
        {
            Sentence = Sentence.Trim();
            BingTrans Result = null;
            if ((Result = BingTransApi.getDataBaseTransResult(Sentence)) != null)
            {
                Console.WriteLine($"DataBase : {Sentence} -> {string.Join(" ", Result.defs)}");
                return Result;
            }
            else
            {
                Result = SentenceTrans(Sentence, "en", "zh-CN");
                BingTransApi.TransResultToDataBase(Result);
                Console.WriteLine($"web : {Sentence} -> {string.Join(" ", Result.defs)}");
                return Result;
            }
        }

    }
}


#region xx
//new JObject(Json);
//var json = JObject. Parse(Json);
//var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(TransResultJson);

//foreach (var item in json["defs"])
//{
//    defs.Add(new defs() { def = item.Value<string>("def"), pos = item.Value<string>("pos") });
//}

//Word = json["word"].Value<string>();

//try
//{
//    Aem = json["pronunciation"].Value<string>("AmE");
//    BrE = json["pronunciation"].Value<string>("BrE");
//}
//catch (Exception) { } 
#endregion
