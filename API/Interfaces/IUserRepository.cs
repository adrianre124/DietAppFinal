using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<MemberDto?> GetMemberAsync(string email);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(Guid id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<AppUser?> GetUserByEmailAsync(string email);
    }
}