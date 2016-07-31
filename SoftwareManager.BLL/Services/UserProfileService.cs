using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.DAL.Contracts;

namespace SoftwareManager.BLL.Services
{

    public class UserProfileService : ServiceBase, IUserProfileService
    {
        public UserProfileService(ISoftwareManagerUoW softwareManagerUoW) : base(softwareManagerUoW)
        {
        }

        public async Task<IUserProfil> GetUserProfileAsync(string userName)
        {
            var user = await SoftwareManagerUoW.ApplicationManagerRepository.FirstOrDefaultAsync( f => f.LoginName == userName );
            if (user != null)
            {
                return Mapper.Map<UserProfile>(user);
            }

            return null;
        }

        public IQueryable<IUserProfil> GetAllUserProfiles()
        {
            return SoftwareManagerUoW.ApplicationManagerRepository.FindAll().ProjectTo<UserProfile>();
        }
    }

}