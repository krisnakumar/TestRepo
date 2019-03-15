using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataInterface.Database
{
    public interface IDatabaseWrapper
    {
        int ExecuteQuery(string command);
        int ExecuteQuery(string[] Commands);
        SqlDataReader ExecuteReader(string command, SqlParameter[] sqlParameters);

        DataSet ExecuteAdapter(string command);

        int ExecuteScalar(string command);

        void CloseConnection();
    }
}
