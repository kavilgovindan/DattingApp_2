using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _Context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext Context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _Context = Context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDtos>> Register(RegisterDtos registerDtos)
        {
            if (await UserExist(registerDtos.username))
            {
                return BadRequest("User name already taken");
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDtos.username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.password)),
                PasswordSalt = hmac.Key
            };

            _Context.Users.Add(user);

            await _Context.SaveChangesAsync();

            return new UserDtos
            {
                Username= user.UserName,
                Token=_tokenService.CreateTokens(user)
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDtos>> login(LoginDtos loginDtos)
        {
            var user = await _Context.Users.SingleOrDefaultAsync(x => x.UserName == loginDtos.username.ToLower());

            if (user == null) { return Unauthorized("Invalid Username"); }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDtos.password));

            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDtos
            {
                Username= user.UserName,
                Token=_tokenService.CreateTokens(user)
            };
        }

        private async Task<bool> UserExist(string username)
        {
            return await _Context.Users.AnyAsync(x => (x.UserName == username.ToLower()));
        }
    }
}