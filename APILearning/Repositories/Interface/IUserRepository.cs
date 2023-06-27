 using APILearning.Entities;
using Microsoft.AspNetCore.Mvc;

namespace APILearning.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<bool> CheckUserIfExist(string email);
        Task<int> AddAsync(AppUser user);
        Task <IEnumerable<AppUser>> GetUsersAsync();
        Task <AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUsersByEmailAsync(string email);
    }
}
