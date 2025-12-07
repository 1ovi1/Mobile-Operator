using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("rate_history")]
    public partial class RateHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("client_id")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Column("rate_id")]
        [ForeignKey("Rate")]
        public int RateId { get; set; }

        [Column("from_date")]
        public DateTime FromDate { get; set; }

        [Column("till_date")]
        public DateTime? TillDate { get; set; }

        public virtual Client Client { get; set; }

        public virtual Rate Rate { get; set; }
    }
}