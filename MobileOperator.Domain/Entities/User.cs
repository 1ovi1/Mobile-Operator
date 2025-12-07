using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MobileOperator.Domain.Entities;

namespace MobileOperator.Domain.Entities
{
    [Table("users")]
    public partial class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [StringLength(50)]
        [Column("password")]
        public string Password { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Client Client { get; set; }
    }
}