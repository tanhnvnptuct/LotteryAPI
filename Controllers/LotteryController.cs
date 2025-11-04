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
            WinCampaign item = new WinCampaign(10, 10, "campaign_test10", "", "", "", 0, "");
            res.Add(item);
            item = new WinCampaign(0, 11, "campaign_test11", "", "", "", 0, "");
            res.Add(item);
            item = new WinCampaign(0, 12, "campaign_12", "", "", "", 0, "");
            res.Add(item);
            //return new string[] { "value1111", "value2" };
            return JsonConvert.SerializeObject(res);
        }



        [HttpPost("[action]")]
        public string GetCurrentCalendar([FromBody] int campaign_id)
        {
            List<WinCalender> res = new List<WinCalender>();
            WinCalender item = new WinCalender(1, campaign_id, "ngay 1--" + campaign_id, "", 0, 0);
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
            WinCalenderDetail item = new WinCalenderDetail(cal_id, "Giai Dac biet", 0, 10, 0,"","",1 );
            res.Add(item);
            item = new WinCalenderDetail(cal_id, "Giai Nhat", 1, 10, 0,"","",1 );
            res.Add(item);
            item = new WinCalenderDetail(cal_id, "Giai Nhi", 2, 10, 0,"","",1 );
            res.Add(item);
            //return "xx"; 
            return JsonConvert.SerializeObject(res);
        }
        




    }
}
