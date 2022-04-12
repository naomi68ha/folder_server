using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace folder_server.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Values")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        //public void InitializeWebStatistic();
        private readonly DAValuesController db = new DAValuesController();
        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}

        [HttpGet]
        [Route("persons")]
        public DataTable GetFullPersons()
        {

            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ServerConnection"].ConnectionString);
                //connection.Open();

                SqlCommand cmd = new SqlCommand();
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDA; connection.Open();
                cmd.CommandText = "SELECT * FROM dbo.persons";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                sqlDA = new SqlDataAdapter(cmd);
                sqlDA.Fill(dataTable);
                connection.Close();

                dataTable.TableName = "persons";

                return dataTable;
            }
        }

        [HttpPost]
        [Route("update_persons")]
        public IHttpActionResult GetUpdatePersons(Value person)
        {
            {

                System.Data.DataTable dtResults = new System.Data.DataTable();
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ServerConnection"].ConnectionString);
                //connection.Open();

                SqlCommand cmd = new SqlCommand("sp_insertperson", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlparameter = new SqlParameter();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("full_name", person.full_name));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("url_linkedin", person.url_linkedin));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("phone", person.phone));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("email", person.email));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("work_position", person.work_position));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("info_type", person.info_type));

                connection.Open();
                System.Data.SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                DataAdapter.Fill(dtResults);
                connection.Close();

                return Ok();

            }

        }

    }
}
