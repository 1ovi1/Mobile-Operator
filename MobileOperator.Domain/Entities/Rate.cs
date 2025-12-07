using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperator.Domain.Entities
{
    [Table("rates")]
    public partial class Rate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rate()
        {
            Client = new HashSet<Client>();
            RateHistory = new HashSet<RateHistory>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Column("city_cost")]
        public decimal? CityCost { get; set; }

        [Column("intercity_cost")]
        public decimal? IntercityCost { get; set; }

        [Column("international_cost")]
        public decimal? InternationalCost { get; set; }

        [Column("gb")]
        public float? GB { get; set; }

        [Column("sms")]
        public int? SMS { get; set; }

        [Column("minutes")]
        public int? Minutes { get; set; }

        [Column("connection_cost")]
        public decimal? ConnectionCost { get; set; }

        [Column("gb_cost")]
        public decimal? GBCost { get; set; }

        [Column("sms_cost")]
        public decimal? SMSCost { get; set; }

        [Column("cost")]
        public decimal? Cost { get; set; }

        [Column("corporate")]
        public bool? Corporate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Client { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RateHistory> RateHistory { get; set; }
    }
}