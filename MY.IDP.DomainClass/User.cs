using System.ComponentModel.DataAnnotations;

namespace MY.IDP.DomainClass
{
   public class User
   {
      [Key]
      [MaxLength(50)]
      public string SubjectId { get; set; }

      [MaxLength(100)]
      [Required]
      public string Username { get; set; }

      [MaxLength(100)]
      public string Password { get; set; }

      [Required]
      public bool IsActive { get; set; }

      public ICollection<UserClaim> UserClaims { get; set; }

      public ICollection<UserLogin> UserLogins { get; set; }   
   } 
}

