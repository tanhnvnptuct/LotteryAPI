 using Newtonsoft.Json;
 public class WinMtTemplate
    {
		public int campaign_id { get; set; }
		public string mt_code { get; set; }
		public string mt_content { get; set; }

		public WinMtTemplate(int campaign_id_, string mt_code_, string mt_content_)
		{
			this.campaign_id = campaign_id_;
			this.mt_code = mt_code_;
			this.mt_content = mt_content_;
		}

		

	}

	[JsonObject(MemberSerialization.OptOut)]
	public class WinMtTemplate_edit
	{
		public int action;
		public WinMtTemplate winmttemplate;

	}
