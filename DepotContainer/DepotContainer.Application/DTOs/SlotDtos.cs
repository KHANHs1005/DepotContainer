namespace DepotContainer.Application.DTOs
{
    public class CreateSlotDto
    {
        public int BlockId { get; set; }  // bắt buộc để liên kết với Block
        public int Bay { get; set; }
        public int Row { get; set; }
        public int Tier { get; set; }
    }

    public class UpdateSlotDto
    {
        public int BlockId { get; set; }  // bắt buộc
        public int Bay { get; set; }
        public int Row { get; set; }
        public int Tier { get; set; }
    }

    public class DeleteSlotDto
    {
        public int Id { get; set; }
    }
}
