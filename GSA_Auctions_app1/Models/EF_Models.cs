using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSA_Auctions_app1.Models
{
    public class GSA_Rootobject
    {
        public Result[] Results { get; set; }
    }

    public class Result
    {
        public string SaleNo { get; set; }
        public int LotNo { get; set; }
        public string AucStartDt { get; set; }
        public string AucEndDt { get; set; }
        public string ItemName { get; set; }
        public string PropertyAddr1 { get; set; }
        public string PropertyAddr2 { get; set; }
        public string PropertyAddr3 { get; set; }
        public string PropertyCity { get; set; }
        public string PropertyState { get; set; }
        public string PropertyZip { get; set; }
        public string AuctionStatus { get; set; }
        public string SaleLocation { get; set; }
        public string LocationOrg { get; set; }
        public string LocationStAddr { get; set; }
        public string LocationCity { get; set; }
        public string LocationST { get; set; }
        public string LocationZip { get; set; }
        public int BiddersCount { get; set; }
        public List<Lotinfo> LotInfo { get; set; }
        public string Instruction1 { get; set; }
        public string Instruction2 { get; set; }
        public string Instruction3 { get; set; }
        public string ContractOfficer { get; set; }
        public string COEmail { get; set; }
        public string COPhone { get; set; }
        public bool Reserve { get; set; }
        public float AucIncrement { get; set; }
        public float HighBidAmount { get; set; }
        public int InactivityTime { get; set; }
        public string AgencyCode { get; set; }
        public string BureauCode { get; set; }
        public string AgencyName { get; set; }
        public string BureauName { get; set; }
        public string ItemDescURL { get; set; }
        [Key]
        public string ImageURL { get; set; }
    }

    public class Lotinfo
    {
        [Key]
        public int Id { get; set; }
        public string LotSequence { get; set; }
        public string LotDescript { get; set; }
    }
}