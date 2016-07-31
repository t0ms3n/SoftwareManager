using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareManager.BLL.Contracts.Models
{
    public interface IVersionModelBase
    {
        byte[] Version { get; }
    }
}
