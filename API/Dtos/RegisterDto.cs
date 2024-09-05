using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; } = string.Empty;
        [Required]
        public required string Email { get; set; } = string.Empty;
        [Required] public string? Gender { get; set; }
        [Required] public string? DateOfBirth { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
    }
}