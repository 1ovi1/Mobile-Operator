using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("ul")]
    public partial class UL
    {
        [Key]
        [Column("user_id")]
        [ForeignKey("Client")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("organization_name")]
        public string OrganizationName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("address")]
        public string Address { get; set; }

        public virtual Client Client { get; set; }
    }
}