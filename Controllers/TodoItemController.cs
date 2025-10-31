using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LotteryAPI.Model;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System.Globalization;

namespace LotteryAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly string _connectionString;
        public TodoItemController(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        // GET: api/values
        [HttpGet("[action]")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("[action]")]
        public string Get1()
        {
            List<Campaing> lst = new List<Campaing>();
            Campaing item = new Campaing();
            item.id = 1;
            item.name = "abc";
            item.lotedate = System.DateTime.Now;
            lst.Add(item);
            item = new Campaing();
            item.id = 2;
            item.name = "xxx";
            item.lotedate = System.DateTime.Now;
            lst.Add(item);
            return JsonConvert.SerializeObject(lst);
        }

     
        [HttpPost("[action]")]
        // POST: api/values
        public IEnumerable<string> Post(Campaing value)
        {
            return new string[] { value.id.ToString(), value.name };
        }



       // [EnableCors("MyPolicy")]
        [HttpGet("[action]")]
        public string GetAllCampaign()
        {
            List<WinCampaign> res = new List<WinCampaign>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_all_campaign";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                //cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_SERVICE_ID", type: OracleDbType.Int16, obj: 90, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                    //WinCampaign item = new WinCampaign(Convert.ToInt32(drd["service_id"]), Convert.ToInt32(drd["campaign_id"]), drd["campaign_name"].ToString(),
                    //    drd.IsDBNull("start_time")  null : drd["start_time"].ToString(), drd.IsDBNull("finish_time")  null : drd["finish_time"].ToString()
                    //    , drd.IsDBNull("notes")  null : drd["notes"].ToString(), drd.IsDBNull("active")  0 : Convert.ToInt32(drd["active"]), drd.IsDBNull("type")  null : drd["type"].ToString());
                    //res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }



        [HttpPost("[action]")]
        public string EditCampaign()
        {
            List<WinCampaign> res = new List<WinCampaign>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_all_campaign";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                //cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_SERVICE_ID", type: OracleDbType.Int16, obj: 90, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                    //WinCampaign item = new WinCampaign(Convert.ToInt32(drd["service_id"]), Convert.ToInt32(drd["campaign_id"]), drd["campaign_name"].ToString(),
                    //    drd.IsDBNull("start_time")  null : drd["start_time"].ToString(), drd.IsDBNull("finish_time")  null : drd["finish_time"].ToString()
                    //    , drd.IsDBNull("notes")  null : drd["notes"].ToString(), drd.IsDBNull("active")  0 : Convert.ToInt32(drd["active"]), drd.IsDBNull("type")  null : drd["type"].ToString());
                    //res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


    }
}
