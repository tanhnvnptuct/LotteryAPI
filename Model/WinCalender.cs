using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryAPI.Model
{
    public class WinCalender
    {
		public int id { get; set; }
		public int campaign_id { get; set; }
		public string lot_name { get; set; }
		public string lot_date { get; set; }
		public int isfinal { get; set; }
		public int status { get; set; }

		public WinCalender(int id_, int campaign_id_, string lot_name_, string lot_date_, int isfinal_, int status_)
		{
			this.id = id_;
			this.campaign_id = campaign_id_;
			this.lot_name = lot_name_;
			this.lot_date = lot_date_;
			this.isfinal = isfinal_;
			this.status = status_;
		}

		

	}

	[JsonObject(MemberSerialization.OptOut)]
	public class WinCalender_edit
	{
		public int action;
		public WinCalender winCalender;

	}

	

	public class PrizeInfo
    {
		public int so_luong_giai;
		public string lot_date;
		public PrizeInfo(int soluong, string p_lotdate)
        {
			so_luong_giai = soluong;
			lot_date = p_lotdate;
        }
    }

}
