using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicDistro.Api.DTO.Write;
using MusicDistro.Api.Settings;
using MusicDistro.Core.Entities.Auth;
using MusicDistro.Core.Services;

namespace MusicDistro.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager; // all data access for users
        private readonly RoleManager<Role> _roleManager; // all data access for roles
        private readonly IOptionsSnapshot<JwtSettings> _jwtSettings;
        private readonly IJwtService _jwtService;
        public AuthController
                            (IMapper mapper,
                            UserManager<User> userManager,
                            RoleManager<Role> roleManager,
                            IOptionsSnapshot<JwtSettings> jwtSettings,
                            IJwtService jwtService)
         =>
            (_mapper, _userManager, _roleManager, _jwtSettings, _jwtService) = (mapper, userManager, roleManager, jwtSettings, jwtService);


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
                // inside if statement
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                return Ok(new
                {
                    Response = "Sign in was successful",
                    JWT = _jwtService.GenerateToken(
                        secret: _jwtSettings.Value.Secret,
                        issuer: _jwtSettings.Value.Issuer,
                        expirationInDays: Convert.ToDouble(_jwtSettings.Value.ExpirationInDays),
                        audience: _jwtSettings.Value.Secret,
                        user: user,
                        roles: userRoles
                        ),
                    UserRoles = userRoles
                });
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
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name should be provided");
            }

            Role role = new Role
            {
                Name = roleName
            };

            IdentityResult roleResult = await _roleManager.CreateAsync(role);
            if (roleResult.Succeeded)
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
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return BadRequest("Please provide an email");
            }

            Role role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                return BadRequest("Role doesn't exist");
            }

            User user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
            {
                return NotFound("User doesn't exist");
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                HttpContext.Response.StatusCode = 201;
                return $"{userEmail} has been assigned a {roleName} role";
            }

            return BadRequest(result.Errors);

        }
    }
}