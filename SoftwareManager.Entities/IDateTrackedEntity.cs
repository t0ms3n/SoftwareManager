using System;

namespace SoftwareManager.Entities
{
    public interface IDateTrackedEntity
    {
        ApplicationManager CreateBy { get; set; }
        int CreateById { get; set; }
        DateTime CreateDate { get; set; }
        ApplicationManager ModifyBy { get; set; }
        int? ModifyById { get; set; }
        DateTime? ModifyDate { get; set; }
    }
}