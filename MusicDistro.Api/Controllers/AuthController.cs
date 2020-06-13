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

namespace MusicDistro.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager; // all data access for users
        private readonly RoleManager<Role> _roleManager; // all data access for roles
        private readonly IOptionsSnapshot<JwtSettings> _jwtSettings;
        public AuthController
                            (IMapper mapper,
                            UserManager<User> userManager,
                            RoleManager<Role> roleManager,
                            IOptionsSnapshot<JwtSettings> jwtSettings)
         =>
            (_mapper, _userManager, _roleManager, _jwtSettings) = (mapper, userManager, roleManager, jwtSettings);


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
                    JWT = GenerateJwt(user, userRoles),
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


        private string GenerateJwt(User user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            IEnumerable<Claim> roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.Value.ExpirationInDays));

            var token = new JwtSecurityToken(
             issuer: _jwtSettings.Value.Issuer,
             audience: _jwtSettings.Value.Issuer,
             claims: claims,
             expires: expires,
             signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}