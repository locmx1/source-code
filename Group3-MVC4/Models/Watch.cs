//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Group3_MVC4.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Watch
    {
        public Watch()
        {
            this.BuyOrders = new HashSet<BuyOrder>();
            this.ExchangeRequests = new HashSet<ExchangeRequest>();
            this.ExchangeRequests1 = new HashSet<ExchangeRequest>();
            this.SellOrders = new HashSet<SellOrder>();
            this.Tags = new HashSet<Tag>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string GlassType { get; set; }
        public string CaseMeterial { get; set; }
        public string MainColor { get; set; }
        public string Images { get; set; }
        public double InTransactionPrice { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string MemberId { get; set; }
        public Nullable<int> ModelId { get; set; }
        public Nullable<int> AvailableAt { get; set; }
    
        public virtual ICollection<BuyOrder> BuyOrders { get; set; }
        public virtual ICollection<ExchangeRequest> ExchangeRequests { get; set; }
        public virtual ICollection<ExchangeRequest> ExchangeRequests1 { get; set; }
        public virtual Member Member { get; set; }
        public virtual Model Model { get; set; }
        public virtual ICollection<SellOrder> SellOrders { get; set; }
        public virtual Store Store { get; set; }
        public virtual Timepeice Timepeice { get; set; }
        public virtual WallLock WallLock { get; set; }
        public virtual WristWatch WristWatch { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}