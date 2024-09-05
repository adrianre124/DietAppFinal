using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {
        public async Task<AppUser?> GetUserByIdAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Email == email);

        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberDto?> GetMemberAsync(string email)
        {
            var query = context.Users
                .Where(x => x.Email == email)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }
    }
}