using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace A246FProject.DAL
{
    public class DbClass
    {
        private static DbClass _instance;
        private static readonly object _lock = new object();

        private readonly string _connectionString;

        private DbClass()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            _connectionString =
                configuration.GetConnectionString("DefaultConnection");
        }

        public static DbClass GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DbClass();
                    }
                }
            }

            return _instance;
        }

        public DataTable ExecuteProcedureWithParameterForDataTable(
            string procedureName,
            SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(procedureName, con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1200;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
            }

            return dt;
        }

        public int ExecuteNonQueryWithParameter(
            string procedureName,
            SqlParameter[] parameters)
        {
            int result = 0;

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                con.Open();

                SqlCommand cmd =
                    new SqlCommand(procedureName, con);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.CommandTimeout = 1200;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        public DataTable ExecuteProcedureForDataTable(string statementName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand oCommand = new SqlCommand();
                    oCommand.Connection = con;
                    oCommand.CommandText = statementName;
                    oCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adpt = new SqlDataAdapter(oCommand);

                    adpt.Fill(dt);
                    oCommand = null;
                    adpt = null;
                }
                catch (Exception e)
                {
                    dt = null;
                    throw new Exception("Error executing query '" + statementName + "' for object.  Cause: " + e.Message);
                }
            }

            return dt;
        }

        public object ExecuteScalarWithParameter(
            string procedureName,
            SqlParameter[] parameters)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                con.Open();

                SqlCommand cmd =
                    new SqlCommand(procedureName, con);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.CommandTimeout = 1200;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteScalar();
            }
        }
    }
}