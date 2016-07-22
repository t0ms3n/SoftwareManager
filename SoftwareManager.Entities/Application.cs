using System;
using System.Collections.Generic;

namespace SoftwareManager.Entities
{
    public class Application : DateTrackedEntity
    {
        public string Name { get; set; }
        public Guid ApplicationIdentifier { get; set; }

        public ICollection<ApplicationApplicationManager> ApplicationApplicationManagers { get; set; }
        public ICollection<ApplicationVersion> ApplicationVersions { get; set; }
    }

}
