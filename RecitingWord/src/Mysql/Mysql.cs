using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class Mysql
    {
        static string connectStr = "";
        public static MySqlConnection mySqlConnection = null;
        public static bool IsHaveDatabase = false;
        public static bool dbConnectInit(string serverName, string portName)
        {
            IsHaveDatabase = false;
            connectStr = "server=127.0.0.1;port=3306;user id=root;password=normanbzhroot;database=dictionary;charset=utf8";//ProgramConfig.Default.MySqlConnectionString;
            mySqlConnection = new MySqlConnection(connectStr);
            Console.WriteLine(connectStr);
            mySqlConnection.Open();
            IsHaveDatabase = true;
            return true;
        }

        public static int Insert(string sql, MySqlParameter[] param)
        {
            int result;
            using (MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection))
            {
                try
                {
                    bool flag = param != null;
                    if (flag)
                    {
                        mySqlCommand.Parameters.AddRange(param);
                    }
                    mySqlCommand.ExecuteNonQuery();
                    result = 0;
                }
                catch (Exception ex)
                {
                    //Log.log(Log.LogLevel.Error, ex.Message + "\r\n" + sql, 1);
                    throw ex;
                }
                return result;
            }
        }

        public static int Insert(string sql)
        {
            if (!IsHaveDatabase) return 0;
            return Insert(sql, null);
        }

        public static int Query(string sql)
        {

            int result;
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection))
                {
                    result = mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataSet Query(string sql, MySqlParameter[] param, string tableName)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, mySqlConnection))
                {
                    bool flag = param != null;
                    if (flag)
                    {
                        mySqlDataAdapter.SelectCommand.Parameters.AddRange(param);
                    }
                    mySqlDataAdapter.Fill(dataSet, tableName);
                }
            }
            catch (Exception ex)
            {
                //Log.log(Log.LogLevel.Error, ex.Message + "\r\n" + sql, 1);
            }
            return dataSet;
        }

        public static DataSet Query(string sql, string tableName)
        {
            if (!IsHaveDatabase) return new DataSet();

            tableName = (string.IsNullOrWhiteSpace(tableName) ? "0" : tableName);
            return Query(sql, null, tableName);
        }

        public static void Update(string sql, string tableName, DataSet ds)
        {
            if (!IsHaveDatabase) return;

            using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, mySqlConnection))
            {
                try
                {
                    MySqlCommandBuilder mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);
                    mySqlDataAdapter.Update(ds, tableName);
                    MySqlCommand updateCommand = mySqlCommandBuilder.GetUpdateCommand();
                }
                catch (Exception ex)
                {
                    //Log.log(Log.LogLevel.Error, ex.Message + "\r\n" + sql, 1);
                    throw ex;
                }
            }
        }

        public static int Update(string sql)
        {
            if (!IsHaveDatabase) return 0;

            return Query(sql);
        }

        public static int Transaction(List<string> sqlList, List<MySqlParameter[]> paraList)
        {
            checked
            {
                MySqlCommand mySqlCommand = new MySqlCommand();
                MySqlTransaction mySqlTransaction = null;
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandTimeout = 120;
                int num = 0;
                try
                {
                    mySqlTransaction = mySqlConnection.BeginTransaction();
                    mySqlCommand.Transaction = mySqlTransaction;
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        mySqlCommand.CommandText = sqlList[i];
                        bool flag = paraList != null && paraList[i] != null;
                        if (flag)
                        {
                            mySqlCommand.Parameters.Clear();
                            mySqlCommand.Parameters.AddRange(paraList[i]);
                        }
                        num = mySqlCommand.ExecuteNonQuery();
                    }
                    mySqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    string text = "";
                    foreach (string current in sqlList)
                    {
                        text = text + current + "\r\n";
                    }
                    //Log.log(Log.LogLevel.Error, ex.Message + "\r\n" + text, 1);
                    try
                    {
                        mySqlTransaction.Rollback();
                    }
                    catch
                    {
                        throw;
                    }
                    throw ex;
                }
                return num;
            }
        }

        public static int Transaction(List<string> sqlList)
        {
            if (!IsHaveDatabase) return 0;

            bool flag = sqlList.Count > 0;
            int result;
            if (flag)
            {
                result = Transaction(sqlList, null);
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}
