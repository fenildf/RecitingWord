using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class JavaScriptHandle
    {
        static JavaScriptHandle _Instance = new JavaScriptHandle();
        public static JavaScriptHandle Instance
        {
            get
            {
                return _Instance;
            }

        }
        private JavaScriptHandle()
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

            CodeDomProvider _provider = new Microsoft.JScript.JScriptCodeProvider();

            CompilerResults results = _provider.CompileAssemblyFromSource(parameters,
            @"package aa
            {
                public class JScript 
                {
                    public static function test(str) 
                    {
                        return 'Hello,'+str;
                    }

                    public static function b(a, b)
                    {
                        for (var d = 0; d < b.length - 2; d += 3) {
                            var c = b.charAt(d + 2),
                                c = ""a"" <= c ? c.charCodeAt(0) - 87 : Number(c),
                                c = ""+"" == b.charAt(d + 1) ? a >>> c : a << c;
                            a = ""+"" == b.charAt(d) ? a + c & 4294967295 : a ^ c
                        }
                        return a
                    }

                    public static function tk(a, TKK) 
                    {
                        for (var e = TKK.split("".""), h = Number(e[0]) || 0, g = [], d = 0, f = 0; f < a.length; f++) {
                            var c = a.charCodeAt(f);
                            128 > c ? g[d++] = c : (2048 > c ? g[d++] = c >> 6 | 192 : (55296 == (c & 64512) && f + 1 < a.length && 56320 == (a.charCodeAt(f + 1) & 64512) ? (c = 65536 + ((c & 1023) << 10) + (a.charCodeAt(++f) & 1023), g[d++] = c >> 18 | 240, g[d++] = c >> 12 & 63 | 128) : g[d++] = c >> 12 | 224, g[d++] = c >> 6 & 63 | 128), g[d++] = c & 63 | 128)
                        }
                        a = h;
                        for (d = 0; d < g.length; d++) a += g[d], a = b(a, ""+-a^+6"");
                        a = b(a, ""+-3^+b+-f"");
                        a ^= Number(e[1]) || 0;
                        0 > a && (a = (a & 2147483647) + 2147483648);
                        a %= 1E6;
                        return a.toString() + ""."" + (a ^ h)
                    }
                }
            }");

            ///注意 不要调用 js的alert(); 否则会丢失 owydldkr.dll
            try
            {
                Assembly assembly = results.CompiledAssembly;

                JavaScriptFunc = assembly.GetType("aa.JScript");
            }
            catch (Exception ex)
            {
            }

            //object obj = JavaScriptFunc.InvokeMember("test", BindingFlags.InvokeMethod,
            //null, null, parame);

            //return obj.ToString();
        }
        public Type JavaScriptFunc { get; set; }
        //public string Execute(params object[] Parame)
        //{
        //    //return JavaScriptFunc.InvokeMember("test", BindingFlags.InvokeMethod, null, null, Parame).ToString();
        //}
        public string tk(params object[] Parame)
        {
            return JavaScriptFunc.InvokeMember("tk", BindingFlags.InvokeMethod, null, null, Parame).ToString();
        }

    }
}
