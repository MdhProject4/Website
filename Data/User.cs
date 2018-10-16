using System.ComponentModel.DataAnnotations;

namespace ProjectFlight.Data
{
	public class User
    {
		/// <summary>
		/// Username, set to whatever by user
		/// </summary>
        [Key]
        [MaxLength(64)]
        public string Username { get; set; }

		/// <summary>
		/// Hashed password
		/// </summary>
        [Required]
        [MaxLength(64)]
        public string Password { get; set; }

		/// <summary>
		/// Email optionally entered by user
		/// </summary>
        [MaxLength(64)]
        public string Email { get; set; }

		/// <summary>
		/// If the user has sacrificed their soul to Satan for a premium account
		/// </summary>
        [Required]
        public bool IsPremium { get; set; }
    }
}
