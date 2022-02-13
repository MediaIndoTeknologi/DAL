﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderKid.DAL.Interface;

namespace WonderKid.Data.Model
{
    public partial class Category:IEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Column("CategoryID")]
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(15)]
        public string CategoryName { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
