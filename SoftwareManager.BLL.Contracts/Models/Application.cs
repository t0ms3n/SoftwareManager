using System;
using System.Collections.Generic;
using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.BLL.Contracts.Models
{
    public class Application : VersionModelBase
    {
        public string Name { get; set; }
        public Guid Identifier { get; set; }
        public IList<ApplicationVersion> ApplicationVersions { get; set; }

        public IList<ApplicationApplicationManager> ApplicationApplicationManagers { get; set; }
    }

}
