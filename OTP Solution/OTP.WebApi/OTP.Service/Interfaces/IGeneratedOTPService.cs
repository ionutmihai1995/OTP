using OTP.Repository.Entities;
using System;
using System.Threading.Tasks;

namespace OTP.Service.Interfaces
{
    public interface IGeneratedOTPService
    {
        Task<GeneratedOTP> AddGeneratedOTP(string userId, Guid secretKeyGuid);
        Task<GeneratedOTP> GetGeneratedOTP(Guid secretKeyGuid);
    }
}
