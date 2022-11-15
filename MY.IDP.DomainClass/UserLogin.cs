using System.ComponentModel.DataAnnotations;

namespace MY.IDP.DomainClass
{
    public class UserLogin
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string SubjectId { get; set; }

        public User User { get; set; }

        [Required]
        [MaxLength(250)]
        public string LoginProvider { get; set; }

        [Required]
        [MaxLength(250)]
        public string ProviderKey { get; set; }
    }
}

