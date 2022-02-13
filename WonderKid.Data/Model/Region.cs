using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderKid.DAL.Interface;

namespace WonderKid.Data.Model
{
    public partial class Region : IEntity
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        [Column("RegionID")]
        public int RegionId { get; set; }
        [Required]
        [Column(TypeName = "nchar(50)")]
        public string RegionDescription { get; set; }

        [InverseProperty("Region")]
        public virtual ICollection<Territory> Territories { get; set; }
    }
}
