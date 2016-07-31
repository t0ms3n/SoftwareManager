using System;

namespace SoftwareManager.DAL.Contracts.Models
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