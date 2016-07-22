using System.Collections.Generic;

namespace SoftwareManager.Entities
{

    public class ApplicationManager : Entity
    {
        public string Name { get; set; }
        public string LoginName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Application> CreatedApplications { get; set; }
        public ICollection<Application> ModifiedApplications { get; set; }
        public ICollection<ApplicationApplicationManager> CreatedApplicationApplicationManagers { get; set; }
        public ICollection<ApplicationApplicationManager> ModifiedApplicationApplicationManagers { get; set; }
        public ICollection<ApplicationApplicationManager> Applications { get; set; }
        public ICollection<ApplicationVersion> CreatedApplicationVersions { get; set; }
        public ICollection<ApplicationVersion> ModifiedApplicationVersions { get; set; }
    }

}