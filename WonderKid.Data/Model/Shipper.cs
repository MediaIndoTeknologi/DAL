﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderKid.DAL.Interface;

namespace WonderKid.Data.Model
{
    public partial class Shipper : IEntity
    {
        public Shipper()
        {
            Orders = new HashSet<Order>();
        }

        [Column("ShipperID")]
        [Key]
        public int ShipperId { get; set; }
        [Required]
        [MaxLength(40)]
        public string CompanyName { get; set; }
        [MaxLength(24)]
        public string Phone { get; set; }

        [InverseProperty("Shipper")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
