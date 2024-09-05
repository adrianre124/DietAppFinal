using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null || user.Email == null) return Unauthorized("Invalid Email");

            if (user.UserName == null) return Unauthorized("Invalid UserName");

            return new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = await tokenService.CreateToken(user),
                Gender = user.Gender
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email)) return BadRequest("Email is taken");

            var user = mapper.Map<AppUser>(registerDto);

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            if (user.UserName == null || user.Email == null) return Unauthorized("Invalid UserName");

            return new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = await tokenService.CreateToken(user),
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string email)
        {
            return await userManager.Users.AnyAsync(x => x.Email == email);
        }
    }
}