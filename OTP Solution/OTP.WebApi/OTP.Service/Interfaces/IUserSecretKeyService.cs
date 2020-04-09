using OTP.Repository.Entities;
using System;
using System.Threading.Tasks;

namespace OTP.Service.Interfaces
{
    public interface IUserSecretKeyService
    {
        Task<UserSecretKey> AddUserSecretKey(string userId, string secretKey);
        Task<bool> IsSecretKeyValid(string userId, Guid secretKeyGuid);
    }
}
