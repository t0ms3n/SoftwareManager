using System;
using System.Collections.Generic;

namespace SoftwareManager.Entities
{

    public class ApplicationVersion : DateTrackedEntity
    {
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
        public string VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

}