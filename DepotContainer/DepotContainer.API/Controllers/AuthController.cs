using DepotContainer.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DepotContainer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;

        public AuthController(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Vui lòng nhập đầy đủ username và password." });

            // 🔍 Tìm trong bảng Staff
            var staff = await _staffRepository.GetByUsernameAsync(request.Username);

            if (staff == null)
                return Unauthorized(new { message = "Tài khoản không tồn tại." });

            if (staff.Password != request.Password)
                return Unauthorized(new { message = "Sai mật khẩu." });

            if (!staff.IsActive)
                return Unauthorized(new { message = "Tài khoản đã bị khóa." });

            return Ok(new
            {
                message = "Đăng nhập thành công",
                staffId = staff.StaffId,
                staffName = staff.StaffName,
                staffType = staff.StaffType,
                username = staff.Username
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
