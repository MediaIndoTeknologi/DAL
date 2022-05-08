using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderKid.DAL6.Interface;

namespace WonderKid.DAL6
{
    public class BaseEntity : IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Column("CreatedBy")]
        [Display(Name = "Creator")]
        public string CreatedBy { get; set; }

        [Column("ModifiedBy")]
        [Display(Name = "Modifier")]
        public string ModifiedBy { get; set; }
        public string CreatedByWithUserNameOnly { get { if (this.CreatedBy != null) { if (this.CreatedBy.Contains("|")) { return this.CreatedBy.Split("|")[0]; } else { return this.CreatedBy; } } else { return "N/A"; } } }
        public string CreatedAtFormated { get { return this.CreatedDate.ToString("dd MMM yyyy HH:mm:ss"); } }
        public string ModifiedByWithUserNameOnly { get { if (this.ModifiedBy != null) { if (this.ModifiedBy.Contains("|")) { return this.ModifiedBy.Split("|")[0]; } else { return this.ModifiedBy; } } else { return "N/A"; } } }
        public string ModifiedAtFormated { get { return this.ModifiedDate.HasValue ? this.ModifiedDate.Value.ToString("dd MMM yyyy HH:mm:ss") : "N/A"; } }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public string DeletedByWithUserNameOnly { get { if (this.DeletedBy != null) { if (this.DeletedBy.Contains("|")) { return this.DeletedBy.Split("|")[0]; } else { return this.CreatedBy; } } else { return "N/A"; } } }
        public string DeletedAtFormated { get { return this.DeletedAt.HasValue ? this.DeletedAt.Value.ToString("dd MMM yyyy HH:mm:ss") : "N/A"; } }
    }
    public class BaseGuidEntity:BaseEntity
    {
        public BaseGuidEntity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
    public class BaseIntEntity : BaseEntity
    {
        [Column(Order = 0)]
        public int Id { get; set; }
    }
    public class BaseStringEntity : BaseEntity
    {
        public string Id { get; set; }
    }
}
