using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShoppingCart_Application_MVC
{
    public class DbConnection
    {
       static SqlConnection _connection;
        protected static SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ShoppingCart_DB"].ConnectionString);
                _connection.Open();
            }
            return _connection;
        }
    }
}