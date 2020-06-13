using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicDistro.Api.DTO.Write;
using MusicDistro.Core.Entities.Auth;

namespace MusicDistro.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager; // all data access for users
        private readonly RoleManager<Role> _roleManager; // all data access for roles
        public AuthController(IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
         =>
            (_mapper, _userManager, _roleManager) = (mapper, userManager, roleManager);


        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignUp(UserSignUp userSignUp)
        {
            User user = _mapper.Map<User>(userSignUp);
            user.UserName = userSignUp.FirstName + userSignUp.LastName + "__" + Guid.NewGuid();

            IdentityResult identityResult = await _userManager.CreateAsync(user, userSignUp.Password);

            if (identityResult.Succeeded)
            {
                HttpContext.Response.StatusCode = 201;
                return user;
            }

            return BadRequest(identityResult.Errors);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserSignIn userSignIn)
        {

            User user = await _userManager.FindByEmailAsync(userSignIn.Email);

            if (
                user != null &&
                (await _userManager.CheckPasswordAsync(user, userSignIn.Password))
               )
            {
                return Ok("sign in was successful");
            }

            return BadRequest("Invalid Credentials were provided");

        }


        /// <summary>
        /// Creates a new role
        /// </summary>
        [ApiConventionMethod(typeof(DefaultApiConventions),
           nameof(DefaultApiConventions.Post))]
        [HttpPost("Role/{roleName}")]
        public async Task<ActionResult<Role>> CreateRole([FromRoute] string roleName)
        {
           if(string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name should be provided");
            }

            Role role = new Role
            {
                Name = roleName
            };

            IdentityResult roleResult = await _roleManager.CreateAsync(role);
            if(roleResult.Succeeded)
            {
                HttpContext.Response.StatusCode = 201;
                return role;
            }

            return BadRequest(roleResult.Errors);
        }



        /// <summary>
        /// Assigns a user to the role
        /// </summary>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Role/{roleName}/{userEmail}")]
        public async Task<ActionResult<string>> AddUserToRole([FromRoute] string userEmail, [FromRoute] string roleName)
        {
           if(string.IsNullOrWhiteSpace(userEmail))
            {
                return BadRequest("Please provide an email");
            }

            Role role = await _roleManager.FindByNameAsync(roleName);
            if(role is null)
            {
                return BadRequest("Role doesn't exist");
            }

            User user = await _userManager.FindByEmailAsync(userEmail);
            if(user is null)
            {
                return NotFound("User doesn't exist");
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);
            if(result.Succeeded)
            {
                HttpContext.Response.StatusCode = 201;
                return $"{userEmail} has been assigned a {roleName} role";
            }

            return BadRequest(result.Errors);
            
        }
    }
}