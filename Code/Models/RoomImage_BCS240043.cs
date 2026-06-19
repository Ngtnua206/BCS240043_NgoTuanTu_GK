using System.ComponentModel.DataAnnotations.Schema;

namespace BTKTGK_BCS240043_NgoTuanTu.Models
{
    public class RoomImage_BCS240043
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsThumbnail { get; set; } = false;

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room_BCS240043? Room { get; set; }
    }
}
