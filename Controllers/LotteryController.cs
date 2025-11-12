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
            WinCalender item = new WinCalender(1, campaign_id, "ngay 123sss--" + campaign_id, "", 0, 0);
            res.Add(item);
            item = new WinCalender(2, campaign_id, "ngay 2--" + campaign_id, "", 0, 0);
            res.Add(item);
            item = new WinCalender(3, campaign_id, "ngay 3--" + campaign_id, "", 0, 0);
            res.Add(item);
            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }


        [HttpPost("[action]")]
        public string GetCurrentDetailCalendar([FromBody] int cal_id)
        {
            List<WinCalenderDetail> res = new List<WinCalenderDetail>();
            WinCalenderDetail item = new WinCalenderDetail(cal_id + 1, "Giai Dac biet", 0, 10, 0, "", "", 1);
            res.Add(item);
            item = new WinCalenderDetail(cal_id + 2, "Giai Nhat", 1, 10, 0, "", "", 1);
            res.Add(item);
            item = new WinCalenderDetail(cal_id + 3, "Giai Nhi", 2, 10, 0, "", "", 1);
            res.Add(item);
            //return "xx"; 
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

            TicketInfo item = new TicketInfo(100, 200, req.campaignId);

            //return "xx"; 
            return JsonConvert.SerializeObject(item);
        }
        

        [HttpPost("[action]")]
        public string doSpin(LotteryReq req)
        {
            List<LotteryRes> res = new List<LotteryRes>();
            for (int i = 0;i<req.campaign_id;i++)
            {
                res.Add(new LotteryRes("mdt:"+req.prize_type+i, "msisdn:"+req.prize_type+i));

            }
            
            
          
            return JsonConvert.SerializeObject(res);
        }
        
        
        




    }
}
