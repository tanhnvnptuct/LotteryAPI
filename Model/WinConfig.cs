 using Newtonsoft.Json;
 public class WinConfig
    {
		public int campaign_id { get; set; }
		public string config_key { get; set; }
		public string config_value { get; set; }
		public string config_type { get; set; }

		public WinConfig(int campaign_id_, string config_key_, string config_value_, string config_type_)
		{
			this.campaign_id = campaign_id_;
			this.config_key = config_key_;
			this.config_value = config_value_;
			this.config_type = config_type_;
		}

		

	}

	[JsonObject(MemberSerialization.OptOut)]
	public class WinConfig_edit
	{
		public int action;
		public WinConfig winconfig;

	}
