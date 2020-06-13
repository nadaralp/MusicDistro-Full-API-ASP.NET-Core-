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
        private readonly UserManager<User> _userManager;
        public AuthController(IMapper mapper, UserManager<User> userManager)
         =>
            (_mapper, _userManager) = (mapper, userManager);


        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost]
        [Route("signup")]
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
        [HttpPost]
        [Route("signin")]
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
    }
}