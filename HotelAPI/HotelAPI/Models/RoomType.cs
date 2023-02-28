using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAPI.Models
{
    [Table("RoomType")]
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Required]
        public decimal Totals { get; set; }
    }
}
