using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("calls")]
    public partial class Call
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("caller_id")]
        [ForeignKey("ClientCaller")]
        public int CallerId { get; set; }

        [StringLength(15)]
        [Column("caller_number")]
        public string CallerNumber { get; set; }

        [Column("called_id")]
        [ForeignKey("ClientCalled")]
        public int? CalledId { get; set; }

        [StringLength(15)]
        [Column("called_number")]
        public string CalledNumber { get; set; }

        [Column("call_time")]
        public DateTime CallTime { get; set; }

        [Column("duration")]
        public TimeSpan Duration { get; set; }

        [Column("type_id")]
        [ForeignKey("CallType")]
        public int TypeId { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

        public virtual Client ClientCaller { get; set; }

        public virtual Client ClientCalled { get; set; }

        public virtual CallType CallType { get; set; }
    }
}