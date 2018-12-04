using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace HiTuanReportTool
{
    public class SQLiteHelper
    {
        SQLiteConnection connection;

        public SQLiteHelper(string connectString)
        {
            connection = new SQLiteConnection(connectString);
        }

        /// <summary>
        /// Create a new SQLiteCommand
        /// </summary>
        /// <param name="commandText">Sql Text</param>
        /// <param name="commandParameters">Parameters</param>
        /// <returns>SQLiteCommand</returns>
        public SQLiteCommand CreateCommand(string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand(commandText, connection);
            if (commandParameters.Length > 0)
            {
                foreach (SQLiteParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            return cmd;
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandParameters">Command parameters.</param>
        /// <returns>SQLite Command</returns>
        public SQLiteCommand CreateCommand(string connectionString, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteConnection cn = new SQLiteConnection(connectionString);

            SQLiteCommand cmd = new SQLiteCommand(commandText, cn);

            if (commandParameters.Length > 0)
            {
                foreach (SQLiteParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            return cmd;
        }
        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="parameterValue">Parameter value.</param>
        /// <returns>SQLiteParameter</returns>
        public SQLiteParameter CreateParameter(string parameterName, System.Data.DbType parameterType, object parameterValue)
        {
            SQLiteParameter parameter = new SQLiteParameter();
            parameter.DbType = parameterType;
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }

        /// <summary>
        /// Shortcut method to execute dataset from SQL Statement and object[] arrray of  parameter values
        /// </summary>
        /// <param name="commandText">Command text.</param>
        /// <param name="paramList">Param list.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, SQLiteParameter[] paramList)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            if (paramList != null && paramList.Length > 0)
            {
                foreach (var parm in paramList)
                    cmd.Parameters.Add(parm);
            }
            DataSet ds = new DataSet();
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();
            connection.Close();
            return ds;
        }

        /// <summary>
        /// Executes the dataset from a populated Command object.
        /// </summary>
        /// <param name="cmd">Fully populated SQLiteCommand</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataset(SQLiteCommand cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(ds);
            da.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            return ds;
        }

        /// <summary>
        /// Executes the dataset in a SQLite Transaction
        /// </summary>
        /// <param name="transaction">SQLiteTransaction. Transaction consists of Connection, Transaction,  /// and Command, all of which must be created prior to making this method call. </param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandParameters">Sqlite Command parameters.</param>
        /// <returns>DataSet</returns>
        /// <remarks>user must examine Transaction Object and handle transaction.connection .Close, etc.</remarks>
        public DataSet ExecuteDataset(SQLiteTransaction transaction, string commandText, params SQLiteParameter[] commandParameters)
        {

            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rolled back or committed, please provide an open transaction.", "transaction");
            IDbCommand cmd = transaction.Connection.CreateCommand();
            cmd.CommandText = commandText;
            foreach (SQLiteParameter parm in commandParameters)
            {
                cmd.Parameters.Add(parm);
            }
            if (transaction.Connection.State == ConnectionState.Closed)
                transaction.Connection.Open();
            DataSet ds = ExecuteDataset((SQLiteCommand)cmd);
            return ds;
        }
    }
}
