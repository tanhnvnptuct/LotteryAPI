 
 public class Go9696Prize
    {

		public int campaign_id { get; set; }
		public int prize_id { get; set; }
		public string prize_date { get; set; }
        public string prize_type { get; set; }
        public int prize_level { get; set; }

		public string win_code { get; set; }
		public string notes { get; set; }
        public string create_time { get; set; }
		public int order { get; set; }
		public int finish { get; set; }

        public string prize_name { get; set; }
		public int reserve { get; set; }
		public int fix_result { get; set; }
        public string mdt_from_date { get; set; }
		public string mdt_to_date { get; set; }
		

		public Go9696Prize(int campaign_id_, int prize_id_, string prize_date_, string prize_type_, int prize_level_, string win_code_, string notes_, string create_time_,
        int order_, int finish_, string prize_name_, int reserve_, int fix_result_, string mdt_from_date_, string mdt_to_date_)
		{
			this.campaign_id = campaign_id_;
			this.prize_id = prize_id_;
			this.prize_date = prize_date_;
            this.prize_type = prize_type_;
			this.prize_level = prize_level_;
			this.win_code = win_code_;
            this.notes = notes_;
			this.create_time = create_time_;
			this.order = order_;
            this.finish = finish_;
			this.prize_name = prize_name_;
			this.reserve = reserve_;
            this.fix_result = fix_result_;
			this.mdt_from_date = mdt_from_date_;
			this.mdt_to_date = mdt_to_date_;
		}

		

	}