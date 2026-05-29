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


    public class GetTrungthuongBig_resp
    {
        public string ngay_trung {get;set;}
        public string msisdn {get;set;}
        public int tra_truoc {get;set;}
        public int id {get;set;}
        public int status_sms {get;set;}
        public string log_date {get;set;}
        public int giai {get;set;}
        public string mdt {get;set;}


        public GetTrungthuongBig_resp (string ngay_trung_, string msisdn_, int tra_truoc_, int id_, int status_sms_, string log_date_, int giai_, string mdt_)
     
        {
            this.ngay_trung = ngay_trung_;
            this.msisdn = msisdn_;
            this.tra_truoc = tra_truoc_;
            this.id = id_;
            this.status_sms = status_sms_;
            this.log_date = log_date_;
            this.mdt = mdt_;
            this.giai= giai_;

        }
    }