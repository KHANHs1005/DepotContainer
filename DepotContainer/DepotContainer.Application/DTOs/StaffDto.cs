namespace DepotContainer.Application.DTOs
{
    public class StaffDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? StaffType { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateStaffDto
    {
        public string StaffName { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? StaffType { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateStaffDto
    {
        public int StaffId { get; set; }                  // bắt buộc có ID
        public string? StaffName { get; set; }            // cho phép null để chỉ cập nhật 1 phần
        public string? ContactPhone { get; set; }
        public string? StaffType { get; set; }
        public bool? IsActive { get; set; }
    }
}
