using System.Collections.Generic;

namespace SoftwareManager.Entities
{

    public class ApplicationManager : Entity
    {
        public string Name { get; set; }
        public string LoginName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public IList<Application> CreatedApplications { get; set; }
        public IList<Application> ModifiedApplications { get; set; }
        public IList<ApplicationApplicationManager> CreatedApplicationApplicationManagers { get; set; }
        public IList<ApplicationApplicationManager> ModifiedApplicationApplicationManagers { get; set; }
        public IList<ApplicationApplicationManager> Applications { get; set; }
        public IList<ApplicationVersion> CreatedApplicationVersions { get; set; }
        public IList<ApplicationVersion> ModifiedApplicationVersions { get; set; }
    }

}