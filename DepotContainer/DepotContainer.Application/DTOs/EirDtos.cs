    using DepotContainer.Domain.Enums;

    namespace DepotContainer.Application.DTOs
    {

        public class EirDto
        {
            public int EirId { get; set; }
            public string EirNumber { get; set; } = string.Empty;
            public EirType Type { get; set; }
            public string ContainerNumber { get; set; } = string.Empty;
            public string CustomerName { get; set; } = string.Empty;
            public string StaffName { get; set; } = string.Empty; // ✅ Đổi StaffId -> StaffName
            public string? PlateNumber { get; set; }
            public int? BatNo { get; set; }
            public DateTime IssueDate { get; set; }
            public DateTime? RegisAt { get; set; }
        }

    // DTO để tạo mới EIR (Request body khi POST)
    public class CreateEirDto
        {
            public string EirNumber { get; set; } = string.Empty;
            public EirType Type { get; set; }                     // Enum: gửi thẳng giá trị enum (GateIn / GateOut)
            public int ContainerId { get; set; }
            public int CustomerId { get; set; }
            public int? StaffId { get; set; }
            public int? BookingId { get; set; }
            public int? SealId { get; set; }
            public string? PlateNumber { get; set; }
            public int? BatNo { get; set; }
            public DateTime? RegisAt { get; set; }                // Thêm cho đồng bộ với entity
        }

        // DTO để cập nhật EIR (Request body khi PUT)
        public class UpdateEirDto
        {
            public int EirId { get; set; }
            public string? EirNumber { get; set; }
            public EirType? Type { get; set; }                    // Nullable enum
            public int? ContainerId { get; set; }
            public int? CustomerId { get; set; }
            public int? StaffId { get; set; }
            public int? BookingId { get; set; }
            public int? SealId { get; set; }
            public string? PlateNumber { get; set; }
            public int? BatNo { get; set; }
            public DateTime? RegisAt { get; set; }                // Có thể cập nhật ngày đăng ký
        }
    }
