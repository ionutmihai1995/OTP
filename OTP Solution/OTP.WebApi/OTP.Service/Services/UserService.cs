using Microsoft.EntityFrameworkCore;
using OTP.Repository.Entities;
using OTP.Repository.Repositories.Interfaces;
using OTP.Service.Interfaces;
using System.Threading.Tasks;

namespace OTP.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMemoryRepository<User> _userRepository;
        public UserService(IMemoryRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsValidUserId(string userId)
        {
            return await _userRepository.SearchQueryable(user => user.UserID == userId).AnyAsync();
        }
    }
}
