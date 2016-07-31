using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.DAL.EF6.Models
{
    public class DbSaveResult : IDbSaveResult
    {
        public int AffectedRows { get; set; }
    }
}
