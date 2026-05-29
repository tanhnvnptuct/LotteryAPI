
public class GetMdtCommon_req
    {
        public string campaign_id {get;set;}
        public string msisdn {get;set;}
    }



    public class GetMdtCommon_resp
    {
        public string prize_date {get;set;}
        public string msisdn {get;set;}
        public string mdt {get;set;}
        public long id {get;set;}
        public string createdate {get;set;}
        public int substype {get;set;}
  

        public GetMdtCommon_resp (string prize_date_, string msisdn_, string mdt_,long id_, string createdate_, int substype_)
        {
            this.prize_date = prize_date_;
            this.msisdn = msisdn_;
            this.mdt = mdt_;
            this.createdate = createdate_;
            this.id=id_;
            this.substype=substype_;

        }
    }