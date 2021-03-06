﻿using Amazon.Lambda.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataInterface.Database
{
    /// <summary>
    ///     Class that handles the database connection(s)
    /// </summary>
    public class DatabaseWrapper : IDatabaseWrapper
    {
        public static string _connectionString { get; set; }
        private SqlConnection sqlConnection = null;


        /// <summary>
        ///     Constructor to get the sql connection string 
        /// </summary>
        public DatabaseWrapper()
        {
            try
            {
                sqlConnection = new SqlConnection(string.Format(_connectionString));
            }
            catch(Exception connectionString)
            {
                LambdaLogger.Log(connectionString.ToString());
            }
        }

        /// <summary>
        ///     This function executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="command">Transact-SQL statement</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteQuery(string command)
        {
            try
            {
                sqlConnection = new SqlConnection(string.Format(_connectionString));
                int rowsaffected = 0;
                SqlTransaction sqlTransaction = null;

                //Creates and returns a SqlCommand object associated with the SqlConnection.
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandTimeout = 120;

                //Check if SQL Connection is Open or Closed
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                    //Starts a database transaction.
                    sqlTransaction = sqlConnection.BeginTransaction();

                    // Must assign both transaction object and connection to Command object for a pending local transaction
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Transaction = sqlTransaction;
                }
                try
                {
                    if (!string.IsNullOrEmpty(command))
                    {
                        sqlCommand.CommandText = command;

                        //Executes a Transact-SQL statement against the connection and returns the number of rows affected.
                        rowsaffected = sqlCommand.ExecuteNonQuery();

                        //Commits the database transaction.
                        if (sqlTransaction != null)
                        {
                            sqlTransaction.Commit();
                        }
                    }
                }
                catch (Exception executeQueryException)
                {
                    LambdaLogger.Log(executeQueryException.ToString());
                }
                return rowsaffected;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     This method executes multiple Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commands">Multiple Array of Transact-SQL statement</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteQuery(string[] Commands)
        {
            int rowsaffected = 0;
            SqlTransaction sqlTransaction = null;

            //Creates and returns a SqlCommand object associated with the SqlConnection.
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            //Check if SQL Connection is Open or Closed
            if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
            {
                //Opens a database connection with the property settings specified by the ConnectionString.
                sqlConnection.Open();

                //Starts a database transaction.
                sqlTransaction = sqlConnection.BeginTransaction();

                // Must assign both transaction object and connection to Command object for a pending local transaction
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
            }
            try
            {
                //iterate through the collection Transact-SQL statement
                foreach (string Command in Commands)
                {
                    if (!string.IsNullOrEmpty(Command))
                    {
                        sqlCommand.CommandText = Command;

                        //Executes a Transact-SQL statement against the connection and returns the number of rows affected.
                        rowsaffected += sqlCommand.ExecuteNonQuery();
                    }
                }
                //Commits the database transaction.
                if (sqlTransaction != null)
                {
                    sqlTransaction.Commit();
                }
            }
            catch (Exception executeQueryException)
            {
                LambdaLogger.Log(executeQueryException.ToString());
            }
            return rowsaffected;
        }

        /// <summary>
        ///     This method executes the Select Command and returns data
        /// </summary>
        /// <param name="command">Transact-SQL statement</param>
        /// <returns>A SqlDataReader object.</returns>
        public SqlDataReader ExecuteReader(string command, SqlParameter[] sqlParameters)
        {
            try
            {
                SqlDataReader sqlDataReader;
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                if (sqlConnection != null)
                {
                    //Creates and returns a SqlCommand object associated with the SqlConnection.
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.Connection = sqlConnection;

                    //Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
                    sqlCommand.CommandText = command;
                    if (sqlParameters != null && sqlParameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameters);
                    }
                    //Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
                    sqlCommand.CommandTimeout = 60;

                    //Sends the CommandText to the Connection and builds a SqlDataReader.
                    sqlDataReader = sqlCommand.ExecuteReader();
                    return sqlDataReader;
                }
                return null;
            }
            catch (Exception executeReaderException)
            {
                LambdaLogger.Log(executeReaderException.ToString());
                return null;
            }
        }


        /// <summary>
        ///     This method executes the Select Command and returns data
        /// </summary>
        /// <param name="command">Transact-SQL statement</param>
        /// <returns>A SqlDataReader object.</returns>
        public virtual IDataReader ExecuteDataReader(string command, SqlParameter[] sqlParameters)
        {
            try
            {
                IDataReader sqlDataReader;
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                if (sqlConnection != null)
                {
                    //Creates and returns a SqlCommand object associated with the SqlConnection.
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.Connection = sqlConnection;

                    //Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
                    sqlCommand.CommandText = command;
                    if (sqlParameters != null && sqlParameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameters);
                    }
                    //Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
                    sqlCommand.CommandTimeout = 60;

                    //Sends the CommandText to the Connection and builds a SqlDataReader.
                    sqlDataReader = sqlCommand.ExecuteReader();
                    return sqlDataReader;
                }
                return null;
            }
            catch (Exception executeReaderException)
            {
                LambdaLogger.Log(executeReaderException.ToString());
                return null;
            }
        }

        /// <summary>
        ///     This method executes the Select Command and returns data
        /// </summary>
        /// <param name="command">Transact-SQL statement</param>
        /// <returns>A SqlDataReader object.</returns>
        public DataSet ExecuteAdapter(string command)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                if (sqlConnection != null)
                {
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, sqlConnection))
                    {
                        sqlDataAdapter.Fill(dataSet);
                    }
                    return dataSet;

                }
                return null;
            }
            catch (Exception executeReaderException)
            {
                LambdaLogger.Log(executeReaderException.ToString());
                return null;
            }
        }


        /// <summary>
        ///     This method executes the Commands and returns the first column of the first row in the result set returned by the query. 
        ///     Additional columns or rows are ignored.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>id</returns>
        public virtual int ExecuteScalar(string command)
        {
            try
            {
                //Check if SQL Connection is Open or Closed
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                //Creates and returns a SqlCommand object associated with the SqlConnection.
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Connection = sqlConnection;

                //Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
                sqlCommand.CommandText = command;

                //Executes the query, and returns the first column of the first row in the result set returned by the query.Additional columns or rows are ignored          
                int result = string.IsNullOrEmpty(Convert.ToString(sqlCommand.ExecuteScalar())) ? 0 : Convert.ToInt32(sqlCommand.ExecuteScalar());
                return result;
            }
            catch (Exception executeScalarException)
            {
                LambdaLogger.Log(executeScalarException.ToString());
            }
            return 0;
        }


        /// <summary>
        ///     This method disposes the database connection.
        /// </summary>
        public virtual void CloseConnection()
        {
            try
            {
                if (sqlConnection != null && (sqlConnection.State & System.Data.ConnectionState.Open) == System.Data.ConnectionState.Open)
                {
                    //Closes the connection to the database. This is the preferred method of closing any open connection.
                    sqlConnection.Close();
                }
            }
            catch (Exception closeConnectionException)
            {
                //The connection-level error that occurred while opening the connection.
                LambdaLogger.Log(closeConnectionException.ToString());
            }
            finally
            {
                //dispose the database Connection
                sqlConnection.Dispose();
            }
        }
    }
}
