namespace DepotContainer.Application.DTOs
{
    public class BookingDetailsDto
    {
        public int BookingId { get; set; }
        public string BookingNumber { get; set; }
        public string ContainerType { get; set; }
        public string CustomerName { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
