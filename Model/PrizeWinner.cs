public class GetResult_req
    {
        public int campaign_id {get;set;}
        public string prize_type {get;set;}
        public string date_from_yyyymmdd {get;set;}
        public string date_to_yyyymmdd {get;set;}
    }



    public class GetResult_resp
    {
        public string msisdn {get;set;}
        public string find_winner_createtime {get;set;}
        public string choose_winner {get;set;}
        public string reason_desc {get;set;}

        public string choose_winner_createtime {get;set;}
        public string prize_level {get;set;}
        public int reserve {get;set;}
        public string win_code {get;set;}

        public string prize_type {get;set;}
        public string prize_date {get;set;}

        public GetResult_resp (string msisdn_, string find_winner_createtime_, string choose_winner_, string reason_desc_, string choose_winner_createtime_, string prize_level_,
        int reserve_, string win_code_, string prize_type_, string prize_date_)
        {
            this.msisdn = msisdn_;
            this.find_winner_createtime = find_winner_createtime_;
            this.choose_winner = choose_winner_;
            this.reason_desc = reason_desc_;
            this.choose_winner_createtime = choose_winner_createtime_;
            this.prize_level = prize_level_;
            this.reserve = reserve_;
            this.win_code = win_code_;
            this.prize_type = prize_type;
            this.prize_date = prize_date_;

        }
    }