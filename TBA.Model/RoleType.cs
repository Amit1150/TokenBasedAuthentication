using System.ComponentModel.DataAnnotations;

namespace TBA.Model
{
    public class RoleType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
