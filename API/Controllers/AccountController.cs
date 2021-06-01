using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _Context;
        public AccountController(DataContext Context)
        {
            _Context = Context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDtos registerDtos)
        {
            if(await UserExist(registerDtos.username))
            {
                return  BadRequest("User name already taken");
            }
            using var hmac= new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDtos.username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.password)),
                PasswordSalt = hmac.Key
            };

            _Context.Users.Add(user);

            await _Context.SaveChangesAsync();

            return user;
            
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> login(LoginDtos loginDtos)
        {
            var user = await _Context.Users.SingleOrDefaultAsync(x => x.UserName == loginDtos.username.ToLower());

            if(user == null) {return Unauthorized("Invalid Username");}

            var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDtos.password));

            for(int i=0; i<ComputedHash.Length; i++)
            {
                if(ComputedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return user;
        }

        private async Task<bool> UserExist( string username)
        {
            return await _Context.Users.AnyAsync(x => (x.UserName == username.ToLower()));
        }
    }
}