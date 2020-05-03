using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.DAL.Entities;
using SocialNetwork.PL.Auth;
using SocialNetwork.PL.ViewModels;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocialNetwork.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationViewModel registrationViewModel)
        {
            var user = new ApplicationUser()
            {
                UserName = registrationViewModel.Login,
                PhoneNumber = registrationViewModel.PhoneNumber,
                Email = registrationViewModel.Email,
            };

            var result = await _userManager.CreateAsync(user, registrationViewModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, login.Password)))
            {
                return Unauthorized();
            }
            
            var claims =
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                };

            var signingKey = JwtAuthOptions.GetSymmetricSecurityKey();

            var token = new JwtSecurityToken
                (
                expires: DateTime.Now.AddMinutes(JwtAuthOptions.LIFETIME_MINUTES),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            
            return Ok
            (
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userName = user.UserName
                }
            );
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserInfoByName()
        {
            var user = _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Email = user.Email;
            userViewModel.UserName = User.Identity.Name;
            userViewModel.PhoneNumber = user.PhoneNumber;
            userViewModel.Password = user.PasswordHash;
            if (user.Avatar != null)
            {
                string imageBase64Data = Convert.ToBase64String(user.Avatar);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                userViewModel.Avatar = imageDataURL;
            }

            return Ok(userViewModel);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel userViewModel)
        {
            var user = _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            user.Email = userViewModel.Email;
            user.PhoneNumber = userViewModel.PhoneNumber;
            user.PasswordHash = userViewModel.Password;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return NoContent();
        }

    }
}