using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.Common.Models;
using Microsoft.Extensions.Options;

namespace SoftwareManager.Common.Services
{
    public interface IApplicationSettingService
    {
        AppSettings AppSettings { get; set; }
    }

}
