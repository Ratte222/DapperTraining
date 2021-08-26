using AutoMapper;
using AuxiliaryLib.Helpers;
using AuxiliaryLib.Extensions;
using BLL.DTO.User;
using BLL.Filters;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IMapper mapper,
            IUserRepository userRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;

        }

        [HttpPost("AddUser")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddUser(UserDTO userDTO)
        {
            _userRepository.Create(_mapper.Map<UserDTO, User>(userDTO));
            return Ok("User created successfully");
        }

        [HttpPut("EditUser")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult EditUser(UserDTO userDTO)
        {
            _userRepository.Update(_mapper.Map<UserDTO, User>(userDTO));
            return Ok("User updated successfully");
        }

        [HttpPut("DeleteUser")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult DeleteUser(int userId)
        {
            _userRepository.Delete(userId);
            return Ok("User updated successfully");
        }

        [HttpGet("GetUser")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetUser(int userId)
        {
            User user = _userRepository.Get(userId);
            if (user == null)
                return NotFound();
            return Ok(_mapper.Map<User, UserDTO>(user));
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(PageResponse<UserDTO>), 200)]        
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetUsers(int? pageLength = null,
            int? pageNumber = null, string fieldOrderBy = null, bool orderByDescending = false)
        {
            PageResponse<UserDTO> pageResponse = new PageResponse<UserDTO>(
                pageLength, pageNumber);
            UserFilter userFilter = new UserFilter()
            {
                FieldOrderBy = fieldOrderBy,
                OrderByDescending = orderByDescending
            };

            var users = _userRepository.GetUsers();
            var query = users.AsQueryable();
            if (String.IsNullOrEmpty(userFilter.FieldOrderBy))
            {
                User user = new User();
                query = query.OrderBy(nameof(user.Name), userFilter.OrderByDescending);
            }
            else
                query = query.OrderBy(userFilter.FieldOrderBy, userFilter.OrderByDescending);

            pageResponse.TotalItems = users.Count();
            pageResponse.Items = _mapper.ProjectTo<UserDTO>(query.Skip(pageResponse.Skip)
                .Take(pageResponse.Take)).ToList();
            return Ok(pageResponse);
        }
    }
}
