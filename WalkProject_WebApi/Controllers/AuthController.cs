using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WalkProject_WebApi.Models.DTO;
using WalkProject_WebApi.Repository;

namespace WalkProject_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRespository tokenRespository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRespository tokenRespository)
        {
            this.userManager = userManager;
            this.tokenRespository = tokenRespository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };
            var IdentityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (IdentityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    //Add Role to the User
                    if (IdentityResult.Succeeded)
                    {                       
                        return Ok("User Was Registerd Pls Login");
                    }
                }
            }
            return BadRequest("Something Want Wrong");

        }

        [HttpPost]
        [Route("Logiin")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if (user != null)
            {
                var CheckPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (CheckPasswordResult)
                {
                    //Create Token JWT
                    var roles = await userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        var jwtToken = tokenRespository.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                    return BadRequest("UserName And Password is Incorrect");
                }
            }
            return BadRequest("UserName And Password is Incorrect");
        }
    }
}
