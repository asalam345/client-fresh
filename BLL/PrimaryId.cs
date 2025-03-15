using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class PrimaryId
    {
        public decimal GetNewId(string tableName, string field)
        {
            try
            {
                decimal newId = 0;
                //string connString = DAL.Connection.conn;
                string sql = "SELECT COALESCE(MAX(" + field + "),0) FROM " + tableName;
                SqlConnection conn = Connection.conn; //new SqlConnection(connString);
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tableName);
                DataTable dt = ds.Tables[tableName];
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                        newId = Convert.ToDecimal(row[col]);
                    //Console.WriteLine("".PadLeft(20, '='));
                }
               //// using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conStr"].ToString()))
               // {
               //     using (SqlCommand cmd = new SqlCommand("spPrimaryId", DAL.Connection.conn))
               //     {
               //         //DAL.Connection.Open();
               //         cmd.CommandText = "SELECT COALESCE(MAX(" + field + "),0) FROM " + tableName;
               //         cmd.ExecuteNonQuery();
               //         var id = cmd.Parameters;
               //     }
               // }

                return newId + 1;

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                Connection.Close();
            }
        }
        //public decimal GetNewId(string tableName, string field)
        //{
        //    try
        //    {
        //        //SqlCommand comm = new SqlCommand();
        //        //comm.Connection = DAL.Connection.conn;
        //        using (SqlConnection con = DAL.Connection.conn)
        //        {
        //            using (SqlCommand cmd = new SqlCommand("spPrimaryId", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                //cmd.Parameters.AddWithValue("@FruitId", int.Parse(txtFruitId.Text.Trim()));
        //                cmd.Parameters.AddWithValue("@TableName", int.Parse(txtFruitId.Text.Trim()));
        //                cmd.Parameters.AddWithValue("@FieldName", int.Parse(txtFruitId.Text.Trim()));




        //                cmd.Parameters.Add("@TableName", SqlDbType.VarChar, 50);
        //                cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;

        //                cmd.Parameters.Add("@FieldName", SqlDbType.VarChar, 50);
        //                cmd.Parameters["@FieldName"].Direction = ParameterDirection.Output;

        //                //con.Open();
        //                cmd.ExecuteNonQuery();
        //                //con.Close();
        //                var id = "Fruit Name: " + cmd.Parameters["@FruitName"].Value.ToString();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        DAL.Connection.Close();
        //    }
        //}
    }
}
