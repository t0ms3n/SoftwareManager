using System;

namespace SoftwareManager.Entities
{

    public abstract class DateTrackedEntity : Entity, IDateTrackedEntity
    {
        public DateTime CreateDate { get; set; }
        public int CreateById { get; set; }
        public ApplicationManager CreateBy { get; set; }

        public DateTime? ModifyDate { get; set; }
        public int? ModifyById { get; set; }
        public ApplicationManager ModifyBy { get; set; }
    }

}