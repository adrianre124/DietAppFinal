using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public required string Gender { get; set; }
        public ICollection<Product> Products{ get; set; } = [];
        public ICollection<DietPlan> DietPlans { get; set; } = [];
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}