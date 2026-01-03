namespace DepotContainer.Application.DTOs
{
    // Dùng cho request đăng nhập
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Dùng cho response khi đăng nhập thành công
    public class AuthResponseDto
    {
        public int StaffId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // nếu bạn có JWT thì dùng, còn không có thể bỏ
    }
}
