using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public static class Insert
    {
        public static int InsertUser(string lastid, params SqlParameter[] coll)
        {

            int lastInserted = 0;

            try
            {


                SqlCommand comm = new SqlCommand();

                comm.Connection = DAL.Connection.conn;


                foreach (var param in coll)
                {
                    comm.Parameters.Add(param);
                }

                SqlParameter paramerters = new SqlParameter();
                paramerters.ParameterName = lastid;
                paramerters.SqlDbType = SqlDbType.Int;
                paramerters.Direction = ParameterDirection.ReturnValue;

                comm.Parameters.Add(paramerters);

                comm.CommandType = CommandType.StoredProcedure;

                comm.CommandText = "InsertNewUser";

                comm.ExecuteNonQuery();

                lastInserted = (int)comm.Parameters[lastid].Value;

            }

            catch (SqlException ex)
            {


            }

            finally {

                if (DAL.Connection.conn.State != ConnectionState.Closed) {

                    DAL.Connection.conn.Close();
                }

            }           

            return lastInserted;

        }

    }
}
