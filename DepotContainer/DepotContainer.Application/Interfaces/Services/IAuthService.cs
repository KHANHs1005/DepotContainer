using DepotContainer.Application.DTOs;
using System.Threading.Tasks;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> LogoutAsync(int staffId);
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}
