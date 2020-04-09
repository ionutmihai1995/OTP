using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OTP.Repository.Entities;
using OTP.Repository.Repositories.Interfaces;
using OTP.Service.Config;
using OTP.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace OTP.Service.Services
{
    public class GeneratedOTPService : IGeneratedOTPService
    {
        private readonly IMemoryRepository<GeneratedOTP> _generatedOTPRepository;
        private readonly OTPGeneratorConfiguration _config;
        public GeneratedOTPService(IMemoryRepository<GeneratedOTP> generatedOTPRepository, IOptions<OTPGeneratorConfiguration> options)
        {
            _generatedOTPRepository = generatedOTPRepository;
            _config = options.Value;
        }
        public async Task<GeneratedOTP> AddGeneratedOTP(string userId, Guid secretKeyGuid)
        {
            var createdDate = DateTime.Now;
            return await _generatedOTPRepository.Create(
                new GeneratedOTP() { 
                    UserId = userId, 
                    SecretKeyGuid = secretKeyGuid,
                    CreatedDate = createdDate, 
                    ExpiredDate = createdDate.AddSeconds(30),
                    GeneratedPassword = GenerateOTPPassword()
                });
        }

        public async Task<GeneratedOTP> GetGeneratedOTP(Guid secretKeyGuid)
        {
            return await _generatedOTPRepository.SearchQueryable(otp => otp.SecretKeyGuid == secretKeyGuid).FirstOrDefaultAsync();
        }

        private string GenerateOTPPassword()
        {
            Random random = new Random(); 
            char[] chars = new char[_config.Length];
            for (int i = 0; i < _config.Length; i++)
            {
                chars[i] = _config.AllowedChars[random.Next(0, _config.AllowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
