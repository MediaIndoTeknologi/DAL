using System;

namespace WonderKid.DAL6.Interface
{
    public interface IEntity
    {
    }
    public interface IBaseEntity : IEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedByWithUserNameOnly { get; }
        public string CreatedAtFormated { get; }
        public string ModifiedByWithUserNameOnly { get; }
        public string ModifiedAtFormated { get; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public string DeletedByWithUserNameOnly { get; }
        public string DeletedAtFormated { get; }

    }
}
