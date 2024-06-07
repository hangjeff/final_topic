using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using DapperProject.Models;
using Dapper;



namespace MobileDAL.cs
{
    public class MobiledetailDAL
    {
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;

        public static SqlConnection connect()
        {
            string connection = "Server=.;Database=Demo;Integrated Security=True;Encrypt=False;";
            SqlConnection con = new SqlConnection(connection);

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            else
            {
                con.Open();
            }
            return con;
        }

        public bool DMLOpperation(string query, object parameters)
        {
            using (var connection = MobiledetailDAL.connect())
            {
                using (var cmd = new SqlCommand(query, connection))
                {
                    // Add parameters to the command
                    if (parameters != null)
                    {
                        var props = parameters.GetType().GetProperties();
                        foreach (var prop in props)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + prop.Name, prop.GetValue(parameters)));
                        }
                    }

                    // Execute the query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Return true if at least one row was affected
                    return rowsAffected > 0;
                }
            }
        }


        public DataTable SelactAll(string query)
        {
            da = new SqlDataAdapter(query, MobiledetailDAL.connect());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        //public DataTable SelactAllWithParam(string query, object parameters)
        //{
        //    using (SqlConnection connection = connect())
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        if (parameters != null)
        //        {
        //            foreach (var prop in parameters.GetType().GetProperties())
        //            {
        //                command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters));
        //            }
        //        }

        //        SqlDataAdapter adapter = new SqlDataAdapter(command);
        //        DataTable dataTable = new DataTable();
        //        adapter.Fill(dataTable);
        //        return dataTable;
        //    }
        //}

        public List<T> Query<T>(string query, object? parameters = null)
        {
            using (SqlConnection connection = connect())
            {
                return connection.Query<T>(query, parameters).ToList();
            }
        }

    }
}
