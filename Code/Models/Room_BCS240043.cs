using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTKTGK_BCS240043_NgoTuanTu.Models
{
    public class Room_BCS240043
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá phải lớn hơn 0")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Diện tích phải lớn hơn 0")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0")]
        public decimal Area { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string? Description { get; set; }

        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType_BCS240043? RoomType { get; set; }

        public ICollection<RoomImage_BCS240043> RoomImages { get; set; } = new List<RoomImage_BCS240043>();
    }
}
