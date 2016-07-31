using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.Common.Models;

namespace SoftwareManager.Common.Services
{
    public interface IApplicationSettingService
    {
        AppSettings AppSettings { get; set; }
    }

}
