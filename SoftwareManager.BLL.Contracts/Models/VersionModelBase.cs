using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SoftwareManager.BLL.Contracts.Models
{
    public class VersionModelBase : ModelBase, IVersionModelBase
    {
        [Timestamp]
        public byte[] Version { get; set; }
    }
}