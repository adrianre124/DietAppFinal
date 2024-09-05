using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController(IUnitOfWork unitOfWork) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await unitOfWork.userRepository.GetUsersAsync();

            return Ok(users);
        }

        // [HttpGet("{username}")]
        // public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
        // {
        //     var user = await unitOfWork.userRepository.GetUserByUsernameAsync(username);

        //     if (user == null) return NotFound();

        //     return user;
        // }

        [HttpGet("{email}")]
        public async Task<ActionResult<MemberDto>> GetUser(string email)
        {
            var user = await unitOfWork.userRepository.GetMemberAsync(email);

            if (user == null) return NotFound();

            return user;
        }
    }
}