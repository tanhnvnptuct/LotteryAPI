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
   
    public class AdminController : ControllerBase
    {
        private readonly string _connectionString;
        public AdminController(IConfiguration _configuratio)
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
        public string Login(User data)
        {
              return "{\"err_code\":200, \"message\":\"OK\", \"username\":\""+data.username+"\"}";

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
        public string get_calendar_campaignid([FromBody]int data)
        {

            string res = "xxx";
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_calendar_campaignid";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_cal_id", type: OracleDbType.Int32, obj: data, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Varchar2, size: 100, obj: res, direction: ParameterDirection.Output));

                cmd_pkg.ExecuteNonQuery();

                res = "0";
                res = cmd_pkg.Parameters["returnds"].Value.ToString();
            }
            //return JsonConvert.SerializeObject("{'err_code':200, 'campaign_id':"+ res + "}");
             return "{\"err_code\":200, \"campaign_id\":"+res+"}";
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


        [HttpGet("[action]")]
        public string GetCalendarDetail(int cal_id)
        {
            List<WinCalenderDetail> res = new List<WinCalenderDetail>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_detail_calendar";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_cal_id", type: OracleDbType.Int16, obj: cal_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                    WinCalenderDetail item = new WinCalenderDetail(Convert.ToInt32(drd["cal_id"]), drd["prize_type"].ToString(), Convert.ToInt32(drd["prize_level"]),
                        Convert.ToInt32(drd["max_prize_level"]), Convert.ToInt32(drd["reserve"]), drd["mdt_from"].ToString(), drd["mdt_to"].ToString(),  Convert.ToInt32(drd["status"]));
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


        [HttpPost("[action]")]
        public string EditCalendarDetail(WinCalenderDetail_edit data)
        {
            OracleCommand cmd_pkg = new OracleCommand();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
              

                cmd_pkg.CommandText = "pkg_web_v2.I_U_D_detail_calendar";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "action", type: OracleDbType.Int32, obj: data.action, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_cal_id", type: OracleDbType.Int32, obj: data.winCalenderDetail.CalId, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_prize_type", type: OracleDbType.Varchar2, obj: data.winCalenderDetail.PrizeType, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_prize_level", type: OracleDbType.Int32, obj: data.winCalenderDetail.PrizeLevel, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_max_prize_level", type: OracleDbType.Int32, obj: data.winCalenderDetail.MaxPrizeLevel, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_reserve", type: OracleDbType.Int32, obj: data.winCalenderDetail.Reserve, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_mdt_from", type: OracleDbType.Varchar2, obj: data.winCalenderDetail.MdtFrom, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_mdt_to", type: OracleDbType.Varchar2, obj: data.winCalenderDetail.MdtTo, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_status", type: OracleDbType.Int32, obj: data.winCalenderDetail.Status, direction: ParameterDirection.Input));
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



        [HttpGet("[action]")]
        public string GetConfig(int campaign_id)
        {
            List<WinConfig> res = new List<WinConfig>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_config";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();

                while (drd.Read())
                {
                    WinConfig item = new WinConfig( Convert.ToInt32(drd["campaign_id"]), drd["config_key"].ToString() , drd["config_value"].ToString(), drd["config_type"].ToString());
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


        [HttpPost("[action]")]
        public string EditConfig(WinConfig_edit data)
        {
            OracleCommand cmd_pkg = new OracleCommand();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
              

                cmd_pkg.CommandText = "pkg_web_v2.I_U_D_config";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "action", type: OracleDbType.Int32, obj: data.action, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int32, obj: data.winconfig.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_config_key", type: OracleDbType.Varchar2, obj: data.winconfig.config_key, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_config_value", type: OracleDbType.Varchar2, obj: data.winconfig.config_value, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_config_type", type: OracleDbType.Varchar2, obj: data.winconfig.config_type, direction: ParameterDirection.Input));
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


        [HttpGet("[action]")]
        public string GetCampaignUser(int campaign_id)
        {
            List<WinUser> res = new List<WinUser>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_campaign_user";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();

                while (drd.Read())
                {
                    WinUser item = new WinUser( Convert.ToInt32(drd["campaign_id"]), drd["msisdn"].ToString() );
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


         [HttpPost("[action]")]
        public string editCampaignUser(WinUser_edit data)
        {
            OracleCommand cmd_pkg = new OracleCommand();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
              

                cmd_pkg.CommandText = "pkg_web_v2.I_U_D_campaign_user";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "action", type: OracleDbType.Int32, obj: data.action, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int32, obj: data.winuser.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_msisdn", type: OracleDbType.Varchar2, obj: data.winuser.msisdn, direction: ParameterDirection.Input));
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


        [HttpGet("[action]")]
        public string GetMTTemplate(int campaign_id)
        {
            List<WinMtTemplate> res = new List<WinMtTemplate>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_mt_template";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();

                while (drd.Read())
                {
                    WinMtTemplate item = new WinMtTemplate( Convert.ToInt32(drd["campaign_id"]), drd["mt_code"].ToString(), drd["mt_content"].ToString());
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


        [HttpPost("[action]")]
        public string EditMTTemplate(WinMtTemplate_edit data)
        {
            OracleCommand cmd_pkg = new OracleCommand();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
              

                cmd_pkg.CommandText = "pkg_web_v2.I_U_D_mt_template";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "action", type: OracleDbType.Int32, obj: data.action, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int32, obj: data.winmttemplate.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_mt_code", type: OracleDbType.Varchar2, obj: data.winmttemplate.mt_code, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_mt_content", type: OracleDbType.Varchar2, obj: data.winmttemplate.mt_content, direction: ParameterDirection.Input));
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


        [HttpGet("[action]")]
        public string GetGo9696Prize(int campaign_id, int cal_id)
        {
            List<Go9696Prize> res = new List<Go9696Prize>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_go9696prize";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_cal_id", type: OracleDbType.Int16, obj: cal_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                    Go9696Prize item = new Go9696Prize( Convert.ToInt32(drd["campaign_id"]), 
                    Convert.ToInt32(drd["prize_id"].ToString()), 
                    drd["prize_date"].ToString(),
                    drd["prize_type"].ToString(), 
                    Convert.ToInt32(drd["prize_level"].ToString()), 
                    drd["win_code"].ToString(),
                    drd["notes"].ToString(), 
                    drd["create_time"].ToString(), 
                    Convert.ToInt32(drd["order_"].ToString()),
                    Convert.ToInt32(drd["finish"]), 
                    drd["prize_name"].ToString(), 
                    Convert.ToInt32(drd["reserve"].ToString()),
                    Convert.ToInt32(drd["fix_result"]), 
                    drd["mdt_from_date"].ToString(), 
                    drd["mdt_to_date"].ToString());
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }

        [HttpGet("[action]")]
        public string DogenGo9696Prize(int campaign_id, int cal_id)
        {
            //List<Go9696Prize> res = new List<Go9696Prize>();
              OracleCommand cmd_pkg = new OracleCommand();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                //OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.dogen_go9696prize";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_cal_id", type: OracleDbType.Int16, obj: cal_id, direction: ParameterDirection.Input));
                OracleParameter param_out = new OracleParameter("returnds", OracleDbType.Varchar2, 1000);
                param_out.Direction = ParameterDirection.Output;
                cmd_pkg.Parameters.Add(param_out);

                cmd_pkg.ExecuteNonQuery();

            }

            //return new string[] { "value1111", "value2" };
           return "{\"err_code\":200, \"message\":\"" + cmd_pkg.Parameters["returnds"].Value.ToString() + "\"}";
        }


        [HttpPost("[action]")]
        public string GetUserRole(User_role data)
        {

            string res = "xxx";
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_user_role";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;

                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_username", type: OracleDbType.Varchar2, obj: data.username, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Int16, size: 100, obj: res, direction: ParameterDirection.Output));

                cmd_pkg.ExecuteNonQuery();

                res = "0";
                res = cmd_pkg.Parameters["returnds"].Value.ToString();
            }
            //return JsonConvert.SerializeObject("{'err_code':200, 'campaign_id':"+ res + "}");
             return "{\"err_code\":200, \"role\":"+res+"}";
        }
        


        [HttpGet("[action]")]
        public string GetMdtCommon(int campaign_id, string msisdn)
        {
            List<GetMdtCommon_resp> res = new List<GetMdtCommon_resp>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_mdt_common";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_campaign_id", type: OracleDbType.Int16, obj: campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_msisdn", type: OracleDbType.Varchar2, obj: msisdn, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                    
                    GetMdtCommon_resp item = new GetMdtCommon_resp(   drd["prize_date"].ToString(),  drd["msisdn"].ToString(), drd["mdt"].ToString(),
                           Convert.ToInt64(drd["id"].ToString()),  drd["createdate"].ToString(), Convert.ToInt32(drd["substype"].ToString()));


                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }
        

        [HttpGet("[action]")]
        public string GetTrungthuongBig(string fromdate, string todate)
        {
            List<GetTrungthuongBig_resp> res = new List<GetTrungthuongBig_resp>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.get_trungthuong_big";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_fromdate", type: OracleDbType.Varchar2, obj: fromdate, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_todate", type: OracleDbType.Varchar2, obj: todate, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();


                while (drd.Read())
                {
                  
                    GetTrungthuongBig_resp item = new GetTrungthuongBig_resp(   drd["ngay_trung"].ToString(),  drd["msisdn"].ToString(),  Convert.ToInt32(drd["tra_truoc"].ToString()),
                           Convert.ToInt32(drd["id"].ToString()), Convert.ToInt32(drd["status_sms"].ToString()),  drd["log_date"].ToString(), Convert.ToInt32(drd["giai"].ToString()), drd["mdt"].ToString());


                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }



    }
}
