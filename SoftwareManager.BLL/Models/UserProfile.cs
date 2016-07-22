using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareManager.BLL.Models
{
    public interface IUserProfil
    {
        int Id { get; set; }
        string Name { get; set; }
        bool IsAdmin { get; set; }
        bool IsActive { get; set; }
    }
    public class UserProfile : IUserProfil
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
