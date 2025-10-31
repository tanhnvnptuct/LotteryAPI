using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;

namespace LotteryAPI.Model
{
	[JsonObject(MemberSerialization.OptOut)]
	public class WinCampaign
	{
		public int? service_id { get; set; }
		public int? campaign_id { get; set; }
		public string? campaign_name { get; set; }
		public string? start_time { get; set; }
		public string? finish_time { get; set; }
		public string? note { get; set; }
		public int? active { get; set; }
		public string? type { get; set; }

		public WinCampaign(int service_id_, int campaign_id_, string campaign_name_, string? start_time_, string? finish_time_, string notes_, int active_, string type_)
		{
			this.service_id = service_id_;
			this.campaign_id = campaign_id_;
			this.campaign_name = campaign_name_;
			this.start_time = start_time_;
			this.finish_time = finish_time_;
			this.note = notes_;
			this.active = active_;
			this.type = type_;
		}

		//public List<WinCampaign> retun_list (OracleDataReader drd)
		//{
		//	List<WinCampaign> res = new List<WinCampaign>();
		//	 while (drd.Read())
		//	{
		//		WinCampaign item = new WinCampaign( Convert.ToInt32( drd["service_id"]), Convert.ToInt32(drd["campaign_id"]), drd["campaign_name"].ToString(),
		//			Convert.ToDateTime( drd["start_time"]), Convert.ToDateTime(drd["finish_time"]), drd["notes"].ToString(), Convert.ToInt32(drd["active"]), drd["type"].ToString());
		//		res.Add(item);
		//	}
		//	return res;

		//}

	}

	[JsonObject(MemberSerialization.OptOut)]
	public class WinCampaign_edit
	{
		public int action;
		public WinCampaign winCampaign;

	}
}
