using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LocalSearcher
{
    public class MySqlHelper
    {


        private static long _lastid;

        public static long LastId { get { return _lastid; } }

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();

        /// <summary>
        /// 执行查询操作，返回DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText, CommandType commandType, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                PrepareCommand(connection, command, null, commandText, commandType, parameters);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    command.Parameters.Clear();
                    return dataSet;
                }
            }
        }

        /// <summary>
        /// 执行查询操作，返回DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                PrepareCommand(connection, command, null, commandText, commandType, parameters);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    command.Parameters.Clear();
                    return dataTable;
                }
            }
        }

        /// <summary>
        /// 执行查询操作，返回MySqlDataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string commandText, CommandType commandType, params MySqlParameter[] parameters)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand();
            try
            {
                PrepareCommand(connection, command, null, commandText, commandType, parameters);
                MySqlDataReader reader = command.ExecuteReader();
                command.Parameters.Clear();
                return reader;
            }
            catch(Exception ex)
            {
                var s = ex.Message;
                command.Parameters.Clear();
                connection.Close();
                throw new Exception();
            }
        }

        /// <summary>
        /// 执行查询操作，返回第一行第一列
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string commandText, CommandType commandType, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                PrepareCommand(connection, command, null, commandText, commandType, parameters);
                object obj = command.ExecuteScalar();
                command.Parameters.Clear();
                return obj;
            }
        }

        /// <summary>
        /// 执行非查询操作，返回受影响的行数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                PrepareCommand(connection, command, null, commandText, commandType, parameters);
                try
                {
                    int result = command.ExecuteNonQuery();
                    _lastid = command.LastInsertedId;
                    command.Parameters.Clear();
                    return result;
                }
                catch
                {
                    command.Parameters.Clear();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行数据库事务，不带参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IList<string> list)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                MySqlTransaction transaction = connection.BeginTransaction();
                PrepareCommand(connection, command, transaction, null, CommandType.Text, null);
                try
                {
                    int count = 0;
                    foreach (string sql in list)
                    {
                        command.CommandText = sql;
                        count += command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return count;
                }
                catch
                {
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行数据库事务，带参数
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, Hashtable ht)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    int count = 0;

                    foreach (DictionaryEntry entity in ht)
                    {
                        MySqlCommand command = new MySqlCommand();
                        PrepareCommand(connection, command, transaction, sql, CommandType.Text, entity.Value as MySqlParameter[]);
                        count += command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return count;
                }
                catch(Exception ex)
                {

                    transaction.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 设置MySqlCommand
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        private static void PrepareCommand(MySqlConnection connection, MySqlCommand command, MySqlTransaction transaction, string commandText, CommandType commandType, params MySqlParameter[] parameters)
        {
            // 建立连接
            if (connection.State != ConnectionState.Open)
            {
                connection.Close();
                connection.Open();
            }
            command.Connection = connection;
            
            // 设置SQL
            if (!string.IsNullOrEmpty(commandText))
            {
                command.CommandText = commandText;
            }
            command.CommandType = commandType;

            // 开启事务
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            // 设置参数
            if (parameters != null)
            {
                foreach (MySqlParameter parameter in parameters)
                {
                    if (parameter.Value == null || parameter.Value.ToString() == "")
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
        }
    }
}

