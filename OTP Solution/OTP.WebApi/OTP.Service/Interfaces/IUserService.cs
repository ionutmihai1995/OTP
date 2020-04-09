using System.Threading.Tasks;

namespace OTP.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsValidUserId(string userId);
    }
}
