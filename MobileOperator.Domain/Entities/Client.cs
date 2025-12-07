using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("clients")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Call = new HashSet<Call>();
            Call1 = new HashSet<Call>();
            RateHistory = new HashSet<RateHistory>();
            ServiceHistory = new HashSet<ServiceHistory>();
            WriteOffs = new HashSet<WriteOff>(); 
        }

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [StringLength(11)]
        [Column("number")]
        public string Number { get; set; }

        [Column("balance")]
        public decimal? Balance { get; set; }

        [Column("rate")]
        public int? RateId { get; set; }

        [Column("minutes")]
        public int Minutes { get; set; }

        [Column("gb")]
        public int? GB { get; set; }

        [Column("sms")]
        public int? SMS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Call> Call { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Call> Call1 { get; set; }

        public virtual Rate Rate { get; set; }

        public virtual User User { get; set; }

        public virtual FL FL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RateHistory> RateHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceHistory> ServiceHistory { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WriteOff> WriteOffs { get; set; }

        public virtual UL UL { get; set; }
    }
}