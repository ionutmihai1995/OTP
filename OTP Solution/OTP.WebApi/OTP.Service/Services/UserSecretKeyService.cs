using Microsoft.EntityFrameworkCore;
using OTP.Repository.Entities;
using OTP.Repository.Repositories.Interfaces;
using OTP.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace OTP.Service.Services
{
    public class UserSecretKeyService : IUserSecretKeyService
    {
        private readonly IMemoryRepository<UserSecretKey> _userSecretKeyRepository;
        public UserSecretKeyService(IMemoryRepository<UserSecretKey> userSecretKeyRepository)
        {
            _userSecretKeyRepository = userSecretKeyRepository;
        }
        public async Task<UserSecretKey> AddUserSecretKey(string userId, string secretKey)
        {
            return await _userSecretKeyRepository.Create(new UserSecretKey() { UserId = userId, SecretKey = secretKey });
        }

        public async Task<bool> IsSecretKeyValid(string userId, Guid secretKeyGuid)
        {
            return await _userSecretKeyRepository.SearchQueryable(s => s.UserId == userId && s.Id == secretKeyGuid).AnyAsync();
        }
    }
}
