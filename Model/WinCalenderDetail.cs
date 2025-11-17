using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryAPI.Model
{
    public class WinCalenderDetail
    {
        public int CalId { get; set; }

        // prize_type VARCHAR2(20 BYTE) NOT NULL
       
        public string PrizeType { get; set; }

        // prize_level NUMBER(8,0) NOT NULL
        
        public int PrizeLevel { get; set; }

        // max_prize_level NUMBER(8,0)
        
        public int MaxPrizeLevel { get; set; }

        // reserve NUMBER(8,0)
        
        public int? Reserve { get; set; }

        // mdt_from DATE
        
        public string MdtFrom { get; set; }

        // mdt_to DATE
        
        public string MdtTo { get; set; }

        // status NUMBER(8,0) DEFAULT 0
       
        public int Status { get; set; }

		public WinCalenderDetail(
            int calId,
            string prizeType,
            int prizeLevel,
            int maxPrizeLevel ,
            int reserve ,
            string mdtFrom ,
            string mdtTo ,
            int status)
        {
            CalId = calId;
            PrizeType = prizeType ;
            PrizeLevel = prizeLevel;
            MaxPrizeLevel = maxPrizeLevel;
            Reserve = reserve;
            MdtFrom = mdtFrom;
            MdtTo = mdtTo;
            Status = status;
        }


      
}
		

	  [JsonObject(MemberSerialization.OptOut)]
    public class WinCalenderDetail_edit
    {
        public int action;
        public WinCalenderDetail winCalenderDetail;

    }

	

}
