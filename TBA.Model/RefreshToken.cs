using System;
using System.ComponentModel.DataAnnotations;
using TBA.Utils;

namespace TBA.Model
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public DeviceType DeviceType { get; set; }
        public string DeviceId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; } 
    }
}
