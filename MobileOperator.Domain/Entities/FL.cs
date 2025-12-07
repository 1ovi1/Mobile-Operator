using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("fl")]
    public partial class FL
    {
        [Key]
        [Column("user_id")]
        [ForeignKey("Client")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("fio")]
        public string FIO { get; set; }

        [Required]
        [StringLength(10)]
        [Column("passport_details")]
        public string PassportDetails { get; set; }

        public virtual Client Client { get; set; }
    }
}