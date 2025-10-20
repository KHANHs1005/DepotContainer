using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace DepotContainer.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStaffRepository _staffRepository;

        public AuthService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        // Đăng nhập
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var staff = await _staffRepository.GetByUsernameAsync(dto.Username);
            if (staff == null) return null;

            // Tạm thời so sánh mật khẩu trực tiếp
            if (staff.Password != dto.Password)
                return null;

            return new AuthResponseDto
            {
                StaffId = staff.StaffId,
                FullName = staff.StaffName,
                Role = staff.StaffType ?? "User",
                Token = "" // nếu chưa dùng JWT thì để trống
            };
        }

        // Đăng xuất
        public Task<bool> LogoutAsync(int staffId)
        {
            // Hiện chưa cần logic gì, chỉ trả về true
            return Task.FromResult(true);
        }

        // Kiểm tra thông tin đăng nhập
        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var staff = await _staffRepository.GetByUsernameAsync(username);
            if (staff == null) return false;
            return staff.Password == password;
        }
    }
}
