using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("write_offs")]
    public partial class WriteOff
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("client_id")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("write_off_date")]
        public DateTime WriteOffDate { get; set; }

        [Required]
        [StringLength(50)]
        [Column("category")]
        public string Category { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public virtual Client Client { get; set; }
    }
}