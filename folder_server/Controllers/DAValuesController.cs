using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace folder_server.Controllers
{
    public class DAValuesController
    {
        private readonly string connection;
        private readonly DbUtils dbUtils;
        /// Initializes a new instance of the <see cref="DAProcess"/> class.     
        public DAValuesController()
        {
            this.connection = ConfigurationManager.ConnectionStrings["ServerConnection"].ConnectionString;
            //this.dbUtils = new DbUtils(this.connection);
        }

        public DataSet ExecuteStoreProcedure(string storedProcedure, List<string> resultsSet, Dictionary<string, object> parameters, int ConnectionTimeout = 30)
        {
            DataSet dsResults = new DataSet();

            if (resultsSet == null || resultsSet.Count == 0)
            {
                resultsSet = new List<string>() { "Table1" };
            }

            using (var Connection = new SqlConnection(this.connection))
            {
                try
                {
                    using (var Command = new SqlCommand(storedProcedure))
                    {
                        Command.Connection = Connection;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandTimeout = ConnectionTimeout;
                        foreach (var p in parameters)
                        {
                            Command.Parameters.Add(new SqlParameter(p.Key, p.Value));
                        }
                        Connection.Open();

                        SqlDataReader DataReader = Command.ExecuteReader();

                        foreach (string dataName in resultsSet)
                        {
                            if (!DataReader.IsClosed)
                            {
                                var data = new DataTable(dataName);
                                data.Load(DataReader);
                                dsResults.Tables.Add(data);
                            }
                        }
                        DataReader.Close();
                        Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    dsResults.Tables.Add(new DataTable("Error"));
                    dsResults.Tables[0].Columns.Add("msg");
                    dsResults.Tables[0].Columns.Add("description");
                    dsResults.Tables[0].Rows.Add(new object[] { ex.Message, ex.ToString() });
                }
                finally
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        Connection.Close();
                    }
                }
            }

            return dsResults;
        }

        //public List<Dictionary<string, string>> GetAllPersons(Dictionary<string, object> parameters)
        //{
        //    return this.dbUtils.ExecuteReaderDictionaryList("dbo.get_all_persons", parameters);
        //}
    }
}
