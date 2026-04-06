 using Newtonsoft.Json;
 public class WinUser
    {
		public int campaign_id { get; set; }
		public string msisdn { get; set; }

		public WinUser(int campaign_id_, string msisdn_)
		{
			this.campaign_id = campaign_id_;
			this.msisdn = msisdn_;
		}

		

	}

	[JsonObject(MemberSerialization.OptOut)]
	public class WinUser_edit
	{
		public int action;
		public WinUser winuser;

	}
