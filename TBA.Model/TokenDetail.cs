using System.ComponentModel.DataAnnotations;
using TBA.Utils;

namespace TBA.Model
{
    public class TokenDetail
    {
        [Key]
        public int Id { get; set; }
        public DeviceType DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string AccessToken { get; set; }
        public int AppUserId { get; set; }
    }
}
