using System.Collections.Generic;

namespace SoftwareManager.BLL.Contracts.Models
{

    public class ApplicationManager : VersionModelBase
    {
        public string Name { get; set; }
        public string LoginName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public IList<Application> CreatedApplications { get; set; }

        public IList<ApplicationVersion> CreatedApplicationVersions { get; set; }

        public IList<ApplicationApplicationManager> MangerOfApplications { get; set; }
    }

}