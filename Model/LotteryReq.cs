using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryAPI.Model
{
    public class LotteryReq
    {
        public int campaign_id { get; set; }
        public string prize_type { get; set; }
        public string prize_level { get; set; }
        public string prize_date { get; set; }

    }

    public class LotteryRes
    {
        public string mdt { get; set; }
        public string msisdn { get; set; }
        public LotteryRes(string p_mdt, string p_msisdn)
        {
            mdt = p_mdt;
            msisdn = p_msisdn;
        }

    }

}
