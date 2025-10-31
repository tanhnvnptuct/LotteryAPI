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
   
    public class LotteryController : ControllerBase
    {
        private readonly string _connectionString;
        public LotteryController(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        [HttpPost("[action]")]
        public string test([FromBody]string data)
        {

            string res = "xxx";
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.test";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_name", type: OracleDbType.Varchar2, obj: data, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Varchar2, size: 100, obj: res, direction: ParameterDirection.Output));

                cmd_pkg.ExecuteNonQuery();

                res = "0";
                res = cmd_pkg.Parameters["returnds"].Value.ToString();
            }
            return JsonConvert.SerializeObject("{'err_code:xxx, message:'"+ res + "'}");
        }

        [HttpPost("[action]")]
        public string testpost(WinCampaign_edit data)
        {
            return "ok123" + data.action.ToString() + "-----------" + data.winCampaign.campaign_name;
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
                    WinCampaign item = new WinCampaign(Convert.ToInt32(drd["service_id"]), Convert.ToInt32(drd["campaign_id"]), drd["campaign_name"].ToString(),
                        drd.IsDBNull("start_time") ? null : drd["start_time"].ToString(), drd.IsDBNull("finish_time") ? null : drd["finish_time"].ToString()
                        , drd.IsDBNull("notes") ? null : drd["notes"].ToString(), drd.IsDBNull("active") ? 0 : Convert.ToInt32(drd["active"]), drd.IsDBNull("type") ? null : drd["type"].ToString());
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }

        


        [HttpPost("[action]")]
        public string EditCampaign(WinCampaign_edit data)
        {
            OracleCommand cmd_pkg = new OracleCommand();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
              

                cmd_pkg.CommandText = "pkg_web_v2.insert_update_delete_campaign";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;
                
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "action", type: OracleDbType.Int32, obj: data.action, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_id", type: OracleDbType.Int32, obj: data.winCampaign.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_name", type: OracleDbType.Varchar2, obj: data.winCampaign.campaign_name, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_start", type: OracleDbType.Varchar2, obj: data.winCampaign.start_time, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_finish", type: OracleDbType.Varchar2, obj: data.winCampaign.finish_time, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_note", type: OracleDbType.Varchar2, obj: data.winCampaign.note, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_active", type: OracleDbType.Int32, obj: data.winCampaign.active, direction: ParameterDirection.Input));
                // cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Varchar2, obj: res, direction: ParameterDirection.Output));
                OracleParameter param_out = new OracleParameter("returnds", OracleDbType.Varchar2, 1000);
                param_out.Direction = ParameterDirection.Output;
                cmd_pkg.Parameters.Add(param_out);
                try
                {
                    cmd_pkg.ExecuteNonQuery();

                }
                catch (Exception ex) { }

            }

            //return new string[] { "value1111", "value2" };
            if (cmd_pkg.Parameters["returnds"].Value.ToString() == "OK")
                return "{\"err_code\":200, \"message\":\"OK\"}";
            else
                return "{\"err_code\":0, \"message\":\"" + cmd_pkg.Parameters["returnds"].Value.ToString() + "\"}";
        }


        [HttpGet("[action]")]
        public string GetCalendar(int campaign_id)
        {
            List<WinCalender> res = new List<WinCalender>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_calendar";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                    WinCalender item = new WinCalender(Convert.ToInt32(drd["id"]), Convert.ToInt32(drd["campaign_id"]),
                        drd["lot_name"].ToString() , drd["lot_date"].ToString(), Convert.ToInt32(drd["isfinal"]),  Convert.ToInt32(drd["status"]));
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


        [HttpPost("[action]")]
        public string EditCalendar(WinCalender_edit data)
        {
            OracleCommand cmd_pkg = new OracleCommand();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
              

                cmd_pkg.CommandText = "pkg_web_v2.insert_update_delete_calendar";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "action", type: OracleDbType.Int32, obj: data.action, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_id", type: OracleDbType.Int32, obj: data.winCalender.id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int32, obj: data.winCalender.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_lot_name", type: OracleDbType.Varchar2, obj: data.winCalender.lot_name, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_lot_date", type: OracleDbType.Varchar2, obj: data.winCalender.lot_date, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_isfinal", type: OracleDbType.Int32, obj: data.winCalender.isfinal, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_status", type: OracleDbType.Int32, obj: data.winCalender.status, direction: ParameterDirection.Input));
                //cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Varchar2,  direction: ParameterDirection.Output));
                OracleParameter param_out = new OracleParameter("returnds", OracleDbType.Varchar2, 1000);
                param_out.Direction = ParameterDirection.Output;
                cmd_pkg.Parameters.Add(param_out);

                cmd_pkg.ExecuteNonQuery();

            }

            //return new string[] { "value1111", "value2" };
            if (cmd_pkg.Parameters["returnds"].Value.ToString() == "OK")
                return "{\"err_code\":200, \"message\":\"OK\"}";
            else
                return "{\"err_code\":0, \"message\":\"" + cmd_pkg.Parameters["returnds"].Value.ToString() + "\"}";
        }


    }
}
