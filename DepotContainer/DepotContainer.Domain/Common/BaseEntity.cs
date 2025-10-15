using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Common
{
    public abstract class BaseEntity
    {
        // Không bắt buộc tên cột Id phải giống DB (chúng ta map bằng [Column] trong mỗi entity)
        public int Id { get; set; }
    }
}
