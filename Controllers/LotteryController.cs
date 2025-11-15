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



        [HttpGet("[action]")]
        public string GetCurrentCampaign()
        {
           List<WinCampaign> res = new List<WinCampaign>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.lottery_get_all_campaign";

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
        public string GetCurrentCalendar([FromBody] int campaign_id)
        {
           List<WinCalender> res = new List<WinCalender>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.lottery_find_calender";

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
        public string GetCurrentDetailCalendar([FromBody] int cal_id)
        {
            List<WinCalenderDetail> res = new List<WinCalenderDetail>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.lottery_find_calender_detail";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "p_cal_id", type: OracleDbType.Int16, obj: cal_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();

    
                while (drd.Read())
                {
                    WinCalenderDetail item = new WinCalenderDetail(Convert.ToInt32(drd["cal_id"]),drd["prize_type"].ToString(), Convert.ToInt32(drd["prize_level"]), Convert.ToInt32(drd["max_prize_level"])
                        , Convert.ToInt32(drd["reserve"]),drd["mdt_from"].ToString(),drd["mdt_to"].ToString(),Convert.ToInt32(drd["status"]));
                    res.Add(item);
                }

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }
        
        [HttpPost("[action]")]
        public string getPrizeInfo([FromBody] int cal_detail_id)
        {
           
            PrizeInfo item = new PrizeInfo(cal_detail_id, "2025-11-15" );
            
            //return "xx"; 
            return JsonConvert.SerializeObject(item);
        }


        [HttpPost("[action]")]
        public string getTicketInfo(TicketInfoReq req)
        {

          //List<WinCalenderDetail> res = new List<WinCalenderDetail>();
			TicketInfo res;
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_common.GET_INFO_TICKET_V2";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_campaign_id", type: OracleDbType.Int16, obj: req.campaignId, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "prs_PRIZE_TYPE", type: OracleDbType.Varchar2, obj: req.prizeType, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "prs_PRIZE_DATE", type: OracleDbType.Varchar2, obj: req.lotDate, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();
				drd.Read();
                res = new TicketInfo(drd["ticketmin"].ToString(),drd["ticketmax"].ToString(),drd["ticketsum"].ToString());

            }

            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }
        

        [HttpPost("[action]")]
        public string doSpin(LotteryReq req)
        {
            List<LotteryRes> res = new List<LotteryRes>();
			using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_common.QUAY_THUONG_V2";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_campaign_id", type: OracleDbType.Int16, obj: req.campaign_id, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_prize_type", type: OracleDbType.Varchar2, obj: req.prize_type, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_PRIZE_LEVEL", type: OracleDbType.Int16, obj: req.prize_level, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_in_dateYYYYMMDD", type: OracleDbType.Varchar2, obj: req.prize_date, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleDataReader drd = cmd_pkg.ExecuteReader();
				while (drd.Read())
                {
                    LotteryRes item = new LotteryRes(drd["mdt"].ToString(),drd["msisdn"].ToString());
                    res.Add(item);
                }
                

            }
			
            return JsonConvert.SerializeObject(res);
        }
        
         [HttpPost("[action]")]
        public string saveResult(SaveResultReq req)
        {
			string res="";
            //List<LotteryRes> res = new List<LotteryRes>();
			using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_common.SAVE_RESULT";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_campaign_id", type: OracleDbType.Int16, obj: req.campaign_id, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_prize_type", type: OracleDbType.Varchar2, obj: req.prize_type, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_PRIZE_LEVEL", type: OracleDbType.Int16, obj: req.prize_level, direction: ParameterDirection.Input));
				cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_in_dateYYYYMMDD", type: OracleDbType.Varchar2, obj: req.prize_date, direction: ParameterDirection.Input));
                //cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Varchar2, direction: ParameterDirection.Output));
				OracleParameter param_out = new OracleParameter("returnds", OracleDbType.Varchar2, 1000);
				param_out.Direction = ParameterDirection.Output;
                cmd_pkg.Parameters.Add(param_out);
				
				cmd_pkg.ExecuteNonQuery();
                res = cmd_pkg.Parameters["returnds"].Value.ToString();


                OracleCommand cmd_pkg_update_calendar = new OracleCommand();

                cmd_pkg_update_calendar.CommandText = "PKG_WEB_V2.lottery_SAVE_RESULT";

                cmd_pkg_update_calendar.Connection = con;
                cmd_pkg_update_calendar.CommandType = CommandType.StoredProcedure;
                cmd_pkg_update_calendar.Parameters.Add(new OracleParameter(parameterName: "ps_campaign_id", type: OracleDbType.Int16, obj: req.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg_update_calendar.Parameters.Add(new OracleParameter(parameterName: "ps_prize_type", type: OracleDbType.Varchar2, obj: req.prize_type, direction: ParameterDirection.Input));
                cmd_pkg_update_calendar.Parameters.Add(new OracleParameter(parameterName: "ps_PRIZE_LEVEL", type: OracleDbType.Int16, obj: req.prize_level, direction: ParameterDirection.Input));
                cmd_pkg_update_calendar.Parameters.Add(new OracleParameter(parameterName: "ps_in_dateYYYYMMDD", type: OracleDbType.Varchar2, obj: req.prize_date, direction: ParameterDirection.Input));
                //cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.Varchar2, direction: ParameterDirection.Output));
                OracleParameter param_out1 = new OracleParameter("returnds", OracleDbType.Varchar2, 1000);
                param_out1.Direction = ParameterDirection.Output;
                cmd_pkg_update_calendar.Parameters.Add(param_out1);
                
                cmd_pkg_update_calendar.ExecuteNonQuery();


            }
			
            //return JsonConvert.SerializeObject(res);
			return res;
        }
        
        

        [HttpPost("[action]")]
        public string GenCodesOff(GenCodesReq req)
        {
            String res="";
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                OracleCommand cmd_pkg = new OracleCommand();

                cmd_pkg.CommandText = "pkg_web_v2.lottery_gen_Lot_Codes";

                cmd_pkg.Connection = con;
                cmd_pkg.Connection.Open();
                cmd_pkg.CommandType = CommandType.StoredProcedure;


                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_campaign_id", type: OracleDbType.Int16, obj: req.campaign_id, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_msisdn", type: OracleDbType.Varchar2, obj: req.msisdn, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_numOfCode", type: OracleDbType.Int16, obj: req.numofcode, direction: ParameterDirection.Input));
                cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_subtype", type: OracleDbType.Int16, obj: req.substype, direction: ParameterDirection.Input));
                 cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "ps_backdays", type: OracleDbType.Int16, obj: req.backdays, direction: ParameterDirection.Input));
                //cmd_pkg.Parameters.Add(new OracleParameter(parameterName: "returnds", type: OracleDbType.RefCursor, direction: ParameterDirection.Output));
                OracleParameter param_out = new OracleParameter("returnds", OracleDbType.Varchar2, 1000);
                param_out.Direction = ParameterDirection.Output;
                cmd_pkg.Parameters.Add(param_out);
                
                cmd_pkg.ExecuteNonQuery();
                res = cmd_pkg.Parameters["returnds"].Value.ToString();

                

            }
            
            return JsonConvert.SerializeObject(res);
        }



    }
}
