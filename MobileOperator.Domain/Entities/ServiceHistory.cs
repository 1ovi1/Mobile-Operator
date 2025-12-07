using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("service_history")]
    public partial class ServiceHistory
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ServiceId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? TillDate { get; set; }

        public virtual Client Client { get; set; }

        public virtual Service Service { get; set; }
    }
}