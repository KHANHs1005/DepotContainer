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
