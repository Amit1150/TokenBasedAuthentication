using System.ComponentModel.DataAnnotations;

namespace TBA.Model
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string ImageName { get; set; }

    }
}
