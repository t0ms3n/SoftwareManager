using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.Common.Models;
using Microsoft.Extensions.Options;

namespace SoftwareManager.Common.Services
{

    public class ApplicationSettingService : IApplicationSettingService
    {
        public AppSettings AppSettings { get; set; }

        public ApplicationSettingService()
        {
            AppSettings = new AppSettings();
            AppSettings.SoftwareManagerConnection =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SoftwareManager;Integrated Security=True;MultipleActiveResultSets=True;App=SoftwareManager DB Migrations;Connection Timeout=9000";
        }

        //public ApplicationSettingService(IOptions<AppSettings> settings)
        //{
        //    if (settings != null)
        //        AppSettings = settings.Value;
        //}
    }

}