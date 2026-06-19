using System.ComponentModel.DataAnnotations;

namespace BTKTGK_BCS240043_NgoTuanTu.Models
{
    public class RoomType_BCS240043
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại phòng không được để trống")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Room_BCS240043> Rooms { get; set; } = new List<Room_BCS240043>();
    }
}
