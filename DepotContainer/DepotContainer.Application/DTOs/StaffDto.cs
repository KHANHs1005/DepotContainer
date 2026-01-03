namespace DepotContainer.Application.DTOs
{
    // Dùng để trả về khi lấy danh sách hoặc chi tiết nhân viên

    public class StaffDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? StaffType { get; set; }
        public bool IsActive { get; set; }
        public string Username { get; set; } = string.Empty; // để Admin thấy tài khoản
    }

    // Dùng khi Admin tạo nhân viên mới (cấp tài khoản)
    public class CreateStaffDto
    {
        public string StaffName { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? StaffType { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
    public class UpdateStaffDto
    {
        public int StaffId { get; set; }                  // bắt buộc có ID
        public string? StaffName { get; set; }            // cho phép null để chỉ cập nhật 1 phần
        public string? ContactPhone { get; set; }
        public string? StaffType { get; set; }
        public bool? IsActive { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    public class DeleteStaffDto
    {
        public int StaffId { get; set; }               // ID nhân viên cần xoá
        public string? Reason { get; set; }            // Lý do xoá (nếu có)
        public int DeletedByAdminId { get; set; }      // Ai thực hiện xoá (Admin)
    }
}
