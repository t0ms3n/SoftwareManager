using System;
using System.Collections.Generic;

namespace SoftwareManager.DAL.Contracts.Models
{
    public class Application : DateTrackedEntity
    {
        public string Name { get; set; }
        public Guid ApplicationIdentifier { get; set; }

        public IList<ApplicationApplicationManager> ApplicationApplicationManagers { get; set; }
        public IList<ApplicationVersion> ApplicationVersions { get; set; }
    }

}
