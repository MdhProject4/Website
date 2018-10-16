using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFlight.Data
{
    public class User
    {
        [Key]
        [MaxLength(64)]
        public string userName { get; set; }
        [Required]
        [MaxLength(64)]
        public string password { get; set; }
        [MaxLength(64)]
        public string email { get; set; }
        [Required]
        public bool isPremium { get; set; }        
    }
}
