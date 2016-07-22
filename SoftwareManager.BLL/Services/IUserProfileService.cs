using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using SoftwareManager.BLL.Models;
using SoftwareManager.DAL.EF6;
using SoftwareManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace SoftwareManager.BLL.Services
{
    public interface IUserProfileService
    {
        Task<IUserProfil> GetUserProfileAsync(string userName);
        IQueryable<IUserProfil> GetAllUserProfiles();
    }

    public class UserProfileService : ServiceBase, IUserProfileService
    {
        private readonly ISoftwareManagerUoW _unitOfWork;

        public UserProfileService(ISoftwareManagerUoW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IUserProfil> GetUserProfileAsync(string userName)
        {
            var user = await _unitOfWork.ApplicationManagerRepository.FirstOrDefaultAsync( f => f.LoginName == userName );
            if (user != null)
            {
                return AutoMapper.Mapper.Map<UserProfile>(user);
            }

            return null;
        }

        public IQueryable<IUserProfil> GetAllUserProfiles()
        {
            return _unitOfWork.ApplicationManagerRepository.FindAll().ProjectTo<UserProfile>();
        }
    }
}
