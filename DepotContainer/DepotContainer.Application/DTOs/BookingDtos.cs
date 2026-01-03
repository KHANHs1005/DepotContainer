namespace DepotContainer.Application.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string? ContSize { get; set; }
        public int? ContQuantity { get; set; }
        public string? OperatorName { get; set; }      // ✅ Giữ lại
        public DateTime? ReleaseExpireDate { get; set; } // ✅ Thay thế cho BookingDate
        public int CusId { get; set; }
    }
    public class BookingDetailDto
    {
        public int BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string? ContSize { get; set; }
        public int? ContQuantity { get; set; }
        public string? OperatorName { get; set; }
        public DateTime? ReleaseExpireDate { get; set; }
        public int CusId { get; set; }
        public string? CustomerName { get; set; }  // Thêm tên khách hàng
        public List<ContainerInBookingDto> Containers { get; set; } = new List<ContainerInBookingDto>();
    }
    public class ContainerInBookingDto
    {
        public int ContId { get; set; }
        public string ContNo { get; set; } = string.Empty;
        public string? ContType { get; set; }      // Loại container (GP/40FT, HC/40FT...)
        public string? ContSize { get; set; }      // Kích thước (20FT, 40FT...)
        public string? OperatorName { get; set; }  // Hãng tàu
        public string? ContStatus { get; set; }    // Empty, Full
        public string? ContCondition { get; set; } // Good, Damaged, Under_repair
        public bool HasEir { get; set; }           // Đã có EIR chưa
        public DateTime? TimeIn { get; set; }      // Thời gian gate-in
        public DateTime? TimeOut { get; set; }     // Thời gian gate-out
        public float? Weight { get; set; }
    }
    public class CreateBookingDto
    {
        public string BookingNumber { get; set; } = string.Empty;
        public string? ContSize { get; set; }
        public int? ContQuantity { get; set; }
        public string? OperatorName { get; set; }
        public DateTime? ReleaseExpireDate { get; set; }
        public int CusId { get; set; }
    }

    public class UpdateBookingDto
    {
        public int BookingId { get; set; }
        public string? ContSize { get; set; }
        public int? ContQuantity { get; set; }
        public string? OperatorName { get; set; }
        public DateTime? ReleaseExpireDate { get; set; }
    }
}
