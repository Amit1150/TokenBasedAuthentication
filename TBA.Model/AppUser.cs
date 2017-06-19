using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace TBA.Model
{
    public class AppUser : IUser<int>
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public bool IsBlocked { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string ImageName { get; set; }

        public string Password { get; set; }

    }
}
