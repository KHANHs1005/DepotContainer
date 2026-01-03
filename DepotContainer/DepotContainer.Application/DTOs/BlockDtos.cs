namespace DepotContainer.Application.DTOs
{
    public class CreateBlockDto
    {
        public string BlockName { get; set; } = string.Empty;
    }

    public class UpdateBlockDto
    {
        public string BlockName { get; set; } = string.Empty;
    }

    public class DeleteBlockDto
    {
        public int Id { get; set; }
    }
}
