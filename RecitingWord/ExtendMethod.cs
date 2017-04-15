using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
/*
 public CL1000Entities(bool AutoDetectChangesEnabled = false)
                   : base("name=CL1000Entities")
        {
            base.Database.Connection.ConnectionString = "server=localhost;user id=root;password=normanbzhroot;persistsecurityinfo=True;database=CL1000";
            this.Configuration.AutoDetectChangesEnabled = AutoDetectChangesEnabled;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("");
            base.OnModelCreating(modelBuilder);
        }

*/
#pragma warning disable CS0168 // 声明了变量，但从未使用过
namespace MVVM
{
    public static class ExtendMethod
    {

        /// <summary>
        /// #xxxxxxx 转换为Color
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        //public static Color ToColor(this string Color)
        //{
        //    return System.Drawing.ColorTranslator.FromHtml(Color);
        //}

        /// <summary>
        /// 把对象序列化并返回相应的字节
        /// </summary>
        /// <param name = "pObj" > 需要序列化的对象 </ param >
        /// < returns > byte[] </ returns >
        static public byte[] SerializeObject(this object pObj)
        {
            if (pObj == null)
                return null;
            System.IO.MemoryStream _memory = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(_memory, pObj);
            _memory.Position = 0;
            byte[] read = new byte[_memory.Length];
            _memory.Read(read, 0, read.Length);
            _memory.Close();
            return read;
        }


        /// <summary>
        /// 把字节反序列化成相应的对象
        /// </summary>
        /// <param name="pBytes">字节流</param>
        /// <returns>object</returns>
        static public T DeserializeObject<T>(this byte[] pBytes)
        {
            T _newOjb;
            if (pBytes == null || pBytes.Length < 1) return default(T);
            System.IO.MemoryStream _memory = new System.IO.MemoryStream(pBytes);
            _memory.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            _newOjb = (T)formatter.Deserialize(_memory);
            _memory.Close();
            return _newOjb;
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Capitalize(this string word)
        {
            if (word.Length <= 1)
                return word;
            return word[0].ToString().ToUpper() + word.Substring(1);
        }

        /// <summary>
        /// 将数组全部元素设置为同样的值
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="value">要设置的值</param>
        /// <returns></returns>
        public static T[] SetAllValues<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }
        /// <summary>
        /// 将数组全部元素设置为同样的值
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="value">要设置的值</param>
        /// <returns></returns>
        public static T[] SetAllValues<T>(this T[] array, int StartIndex, T value)
        {
            if (StartIndex >= array.Length)
            {
                throw new Exception("起始位置大于数组长度");
            }
            for (int i = StartIndex; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }
        /// <summary>
        /// 对象转换为Xml 保持对象状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T o) where T : new()
        {
            string retVal;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(typeof(T));
                xs.Serialize(ms, o);
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                retVal = sr.ReadToEnd();
            }
            return retVal;
        }
        /// <summary>
        /// 计算position占total的百分比，常用于统计方面。
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this double position, int total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="str"></param>
        public static byte[] Compress(this byte[] inputBytes)
        {
            MemoryStream outStream = null;
            GZipStream zipStream = null;
            byte[] ret = null;
            try
            {
                outStream = new MemoryStream();
                zipStream = new GZipStream(outStream, CompressionMode.Compress, true);
                zipStream.Write(inputBytes, 0, inputBytes.Length);
                zipStream.Close(); //很重要，必须关闭，否则无法正确解压
                ret = outStream.ToArray();
            }
            catch (Exception exx)
            {
                //Logger.Log.Error(exx);
            }

            return ret;
        }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        /// <param name="str"></param>
        public static byte[] Decompress(this byte[] inputBytes)
        {
            MemoryStream inputStream = null;
            MemoryStream outStream = null;
            byte[] ret = null;
            try
            {
                inputStream = new MemoryStream(inputBytes);
                outStream = new MemoryStream();

                GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress);

                zipStream.CopyTo(outStream);
                zipStream.Close();
                ret =  outStream.ToArray();

            }
            catch (Exception  exx)
            {
                //Logger.Log.Error(exx);
                //throw;
            }

            return ret;
        }
        // <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Compress(this string input)
        {
            byte[] inputBytes = Encoding.Default.GetBytes(input);
            byte[] result = Compress(inputBytes);
            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decompress(this string input)
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] depressBytes = Decompress(inputBytes);
            return Encoding.Default.GetString(depressBytes);
        }
        /// <summary>
        /// 计算Point的平方和
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        //public static int TowPowSum(this Point point)
        //{
        //    return point.X * point.X + point.Y * point.Y;
        //}
        
        public static T DeepCopy<T>(this T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
                catch { throw; }
            }
            return (T)retval;
        }

        public static string[] Lines(this string value, params char[] Split)
        {
            return value.Split(Split != null ? Split : new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string[] Lines(this string value, string Split = "\r\n")
        {
            return value.Split(new string[] { Split }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string Trim(this string value, params char[] trim)
        {
            return value.Trim(trim);
        }
        public static bool IsInteger(this string value)
        {
            int i;
            return int.TryParse(value, out i);
        }
        public static int GetHighBitCount<T>(this T Va) where T : struct
        {
            long value = 0;
            if (Va is int || Va is int?)
                value = (int)(Va as int?);
            else if (Va is uint || Va is uint?)
                value = (uint)(Va as uint?);
            else if (Va is short || Va is short?)
                value = (short)(Va as short?);
            else if (Va is ushort || Va is ushort?)
                value = (ushort)(Va as ushort?);
            else if (Va is byte || Va is byte?)
                value = (byte)(Va as byte?);
            else if (Va is char || Va is char?)
                value = (char)(Va as char?);
            else
                throw new Exception();// Log.log(Log.LogLevel.Error, "系统表示很害怕,不知道应该怎么做");
            int HighBitCount = 0;
            for (int i = 0; i < System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)) * 8; i++)
            {
                if ((value & 0x01) > 0) HighBitCount++;
                value >>= 1;
            }
            return HighBitCount;
        }
        /// <summary>
        /// 结构体 值类型转byte数组
        /// BitConverter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes<T>(this T structObj) where T : struct
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];

            IntPtr structPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.StructureToPtr(structObj, structPtr, true);
            System.Runtime.InteropServices.Marshal.Copy(structPtr, bytes, 0, size);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(structPtr);
            return bytes;
        }
        /// <summary>
        /// 结构体List转byte数组
        /// BitConverter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes<T>(this IEnumerable<T> structObjList) where T : struct
        {
            byte[] bytes = new byte[] { };
            if (structObjList == null) return bytes;
            if (structObjList.Count() <= 0) return bytes;
            int size = System.Runtime.InteropServices.Marshal.SizeOf(structObjList.GetEnumerator().Current);
            List<byte> buffer = new List<byte>(size * structObjList.Count());
            IntPtr structPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);//分配
            foreach (var item in structObjList)
            {
                bytes = new byte[size];
                System.Runtime.InteropServices.Marshal.StructureToPtr(item, structPtr, true);
                System.Runtime.InteropServices.Marshal.Copy(structPtr, bytes, 0, size);
                buffer.AddRange(bytes);
            }
            System.Runtime.InteropServices.Marshal.FreeHGlobal(structPtr);              //释放
            return buffer.ToArray();
        }
        /// <summary>
        /// byte数组转 结构体 值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T BytesToStuct<T>(this byte[] bytes,int offset = 0) where T : struct
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(default(T));
            if (size > bytes.Length)
            {
                return default(T);
            }
            IntPtr structPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.Copy(bytes, offset, structPtr, size);
            T obj = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(structPtr, typeof(T));
            System.Runtime.InteropServices.Marshal.FreeHGlobal(structPtr);

            return obj;
        }


        /// <summary>
        /// byte数组转 结构体List 值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static List<T> BytesToStuctList<T>(this byte[] bytes, int offset = 0,int byteCount = 0) where T : struct
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(default(T));
            byteCount = byteCount <= 0 ? bytes.Length : byteCount;
            if (size > byteCount) return default(List<T>);
            int count = byteCount / size;
            List<T> StuctList = new List<T>(count);

            IntPtr structPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);//分配
            for (int i = 0; i < count; i++)
            {
                System.Runtime.InteropServices.Marshal.Copy(bytes, offset + i * size, structPtr, size);
                StuctList.Add((T)System.Runtime.InteropServices.Marshal.PtrToStructure(structPtr, typeof(T)));
            }
            System.Runtime.InteropServices.Marshal.FreeHGlobal(structPtr);              //释放
            return StuctList;
        }
        //public static TF DeepCopy<T, TF>(T original)
        //{
        //    string json = JsonSerialize(original);
        //    var result = JsonDeserialize<TF>(json);
        //    return result;
        //}
        public static string GetHash(this string bytes, HashType type = HashType.MD5)
        {
            return GetHashCodeString(Encoding.UTF8.GetBytes(bytes), type);
        }
        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null) return "";
            if (bytes.Length <= 0) return "";

            StringBuilder sb = new StringBuilder(bytes.Length * 5);
            foreach (var Item in bytes)
            {
                sb.AppendFormat("0x{0:x2} ", Item);
            }
            return sb.ToString();
        }

        public static string GetHashCodeString(this byte[] bytes, HashType type = HashType.MD5)
        {
            switch (type)
            {
                case HashType.SHA1:
                    SHA1 sha1 = new SHA1CryptoServiceProvider();
                    return BitConverter.ToString(sha1.ComputeHash(bytes));
                case HashType.SHA256:
                    SHA256 sha256 = new SHA256CryptoServiceProvider();
                    return BitConverter.ToString(sha256.ComputeHash(bytes));
                case HashType.SHA384:
                    SHA384 sha384 = new SHA384CryptoServiceProvider();
                    return BitConverter.ToString(sha384.ComputeHash(bytes));
                case HashType.SHA512:
                    SHA512 sha512 = new SHA512CryptoServiceProvider();
                    return BitConverter.ToString(sha512.ComputeHash(bytes));
                default:
                    MD5 md5 = MD5.Create();
                    return BitConverter.ToString(md5.ComputeHash(bytes));
            }
        }

        public static float ToFloat(this string charstring)
        {
            float Numer;
            if (float.TryParse(charstring, out Numer))
                return Numer;
            else
                return 0f;
        }

        public static T ToNumer<T>(this string str)
        {
            T OutValue = default(T);
            var TryParse = typeof(T).GetMethod("TryParse", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, Type.DefaultBinder, new Type[] { typeof(string), typeof(T).MakeByRefType() }, null);
            if (TryParse != null)
            {
                var param = new object[] { str, OutValue };
                if ((bool)TryParse.Invoke(null, param))
                {

                    return (T)param[1];
                }
            }
            return OutValue;
        }
        public static T ToNumer<T>(this string str, T DefaultValue)
        {
            var TryParse = typeof(T).GetMethod("TryParse", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, Type.DefaultBinder, new Type[] { typeof(string), typeof(T).MakeByRefType() }, null);
            if (TryParse != null)
            {
                var param = new object[] { str, DefaultValue };
                if ((bool)TryParse.Invoke(null, param))
                {

                    return (T)param[1];
                }
            }
            return DefaultValue;
        }

        public static T ConvertTo<T>(this object Obj)
        {
            if (Obj == null)
            {
                return default(T);
            }
            var typeConverter1 = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if (typeConverter1.CanConvertFrom(Obj.GetType()))
            {
                return (T)typeConverter1.ConvertFrom(Obj);
            }

            var typeConverter2 = System.ComponentModel.TypeDescriptor.GetConverter(Obj.GetType());
            if (typeConverter2.CanConvertFrom(typeof(T)))
            {
                return (T)typeConverter1.ConvertTo(Obj, typeof(T));
            }

            return (T)Convert.ChangeType(Obj, typeof(T));
        }

        /// <summary>
        /// 测试MySql查询结果是否可用 
        /// 默认至少存在一行数据时返回True
        /// </summary>
        /// <param name="dataSet">待测试的数据集</param>
        /// <param name="TablesCount">最少表数量</param>
        /// <param name="RowCount">最少行数量</param>
        /// <returns></returns>
        public static bool Available(this System.Data.DataSet dataSet, int TablesCount = 1, int RowCount = 1)
        {
            try
            {
                if (dataSet == null) return false;
                if (dataSet.Tables == null) return false;
                if (dataSet.Tables.Count < TablesCount) return false;
                if (dataSet.Tables[0] == null) return false;
                if (dataSet.Tables[0].Rows == null) return false;
                if (dataSet.Tables[0].Rows.Count < RowCount) return false;
                if (dataSet.Tables[0].Rows[0] == null) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 测试MySql查询结果是否可用 
        /// 默认至少存在一行数据时返回True
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="RowCount">最少行数量</param>
        /// <returns></returns>
        public static bool Available(this System.Data.DataTable dataTable, int RowCount = 1)
        {
            try
            {
                if (dataTable == null) return false;
                if (dataTable.Rows == null) return false;
                if (dataTable.Rows.Count < RowCount) return false;
                if (dataTable.Rows[0] == null) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 求集合的交集
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="Collection"></param>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<IEnumerable<TSource>> Collection, IEnumerable<IEnumerable<TSource>> Collections)
        {
            IEnumerable<TSource> CollectionIntersect = Collection.First();
            foreach (var Item in Collections)
            {
                CollectionIntersect = CollectionIntersect.Intersect(Item);
            }
            return CollectionIntersect;
        }
        /// <summary>
        /// 求集合的交集
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="Collection"></param>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> Collection, IEnumerable<IEnumerable<TSource>> Collections)
        {
            IEnumerable<TSource> CollectionIntersect = Collection;
            foreach (var Item in Collections)
            {
                CollectionIntersect = CollectionIntersect.Intersect(Item);
            }
            return CollectionIntersect;
        }
        /// <summary>
        /// 追加 记得要 Array = Array.Append(Data);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="AppendData"></param>
        /// <returns></returns>
        public static T[] Append<T>(this T[] Data, T[] AppendData)
        {
            var DataList = Data.ToList();
            DataList.AddRange(AppendData);
            return  DataList.ToArray();
        }
        /// <summary>
        /// 追加 记得要 Array = Array.Append(Data);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="AppendData"></param>
        /// <returns></returns>
        //public static T[] Append<T>(this T[] Data, T[] AppendData, int Offset = 0, int Count = 0)
        //{
        //    Count = Count <= 0 ? AppendData.Length : Count;
        //    var AppendBuffer = new ArraySegment<T>(AppendData, Offset, Count);
        //    var DataList = Data.ToList();
        //    DataList.AddRange(AppendBuffer.ToArray());
        //    return DataList.ToArray();
        //}
        #region Stream
        //public static void Write(this Stream stream, Communication.Commands cmd, object data)
        //{
        //    var WriteBuffer = data.SerializeObject();
        //    stream.Write(Communication.Command.Create(cmd, WriteBuffer.Length).StructToBytes().Append(WriteBuffer));
        //}
        //public static void Write(this Stream stream, Communication.Commands cmd, byte[] WriteBuffer)
        //{
        //    stream.Write(Communication.Command.Create(cmd, WriteBuffer.Length).StructToBytes().Append(WriteBuffer));
        //}
        public static void Write(this Stream stream, byte[] WriteBuffer)
        {
            stream.Write(WriteBuffer, 0, WriteBuffer.Length);
        }
        public static int Read(this Stream stream, byte[] WriteBuffer)
        {
            try
            {
                return stream.Read(WriteBuffer, 0, WriteBuffer.Length);
            }
            catch (Exception)
            {
                return 0;
            }
        } 
        #endregion
        public static int SizeOf(this ValueType Data)
        {
            return Marshal.SizeOf(Data);
        }
        public static int SizeOf(this Type type)
        {
            return Marshal.SizeOf(type);
        }


        public static string GetEnumeratorString<T>(this T collection,string separator = ",") where T : IEnumerable<T>
        {
            return string.Join(separator, collection);
        }
    }

    public enum HashType
    {
        MD5,//速度略快,关键敏感场合不建议用
        SHA1,//关键敏感场合不建议用
        SHA256,//
        SHA384,
        SHA512
    }
}

