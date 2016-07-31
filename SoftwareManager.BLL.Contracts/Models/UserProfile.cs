namespace SoftwareManager.BLL.Contracts.Models
{
    public interface IUserProfil : IModelBase
    {
        string Name { get; set; }
        bool IsAdmin { get; set; }
        bool IsActive { get; set; }
    }

    public class UserProfile : ModelBase, IUserProfil
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
