using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("admins")]
    public partial class Admin
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        public virtual User User { get; set; }
    }
}