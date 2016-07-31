using System;

namespace SoftwareManager.BLL.Contracts.Models
{
    public class ApplicationVersion : VersionModelBase
    {
        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
        public string VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
