using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MemberDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }
        public required string Gender { get; set; }
    }
}