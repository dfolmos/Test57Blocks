using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test57Blocks.DTO;
using Test57Blocks.Models;
using Test57Blocks.Tools;

namespace Test57Blocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly _57BlocksContext db;
        public UsersController(IConfiguration configuration, _57BlocksContext context)
        {
            _configuration = configuration;
            db = context;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<TransactionResultDTO>> Register([FromBody] RegisterUserDTO newUser)
        {
            TransactionResultDTO result = new TransactionResultDTO();
            if (db.Users.Where(x => x.UserEmail == newUser.UserEmail).Any())
            {
                result.Success = false;
                result.Errors.Add("The email already exits");

            }
            else
            {
                try
                {
                    User user = new User();
                    user.UserEmail = newUser.UserEmail;
                    user.UserPassword = Encryption.EncriptString(newUser.Password);
                    user.UserName = newUser.Name;
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    result.Data = await BuildToken(user);
                    result.Success = true;
                    result.Message = "User created succesfully";
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Errors.Add(ex.Message);
                }
            }
            return result;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginInfo)
        {

            TransactionResultDTO result = new TransactionResultDTO();
            User user = db.Users.Where(x => x.UserEmail == loginInfo.UserEmail).FirstOrDefault();
            if (user == null)
            {
                result.Success = false;
                result.Errors.Add("The user dont exits");

            }
            else
            {
                if (user.UserPassword == Encryption.EncriptString(loginInfo.Password))
                {
                    result.Data = await BuildToken(user);
                    result.Success = true;
                    result.Message = "Access allowed";
                }
                else
                {
                    result.Success = false;
                    result.Errors.Add("Incorrect password");
                }
            }

            return Ok(result);
        }
        private async Task<TokenDTO> BuildToken(User user)
        {
            List<Claim> claims = new List<Claim>()
        {
            new Claim("name",user.UserName),
            new Claim("idUser",user.IdUser.ToString()),
            new Claim("email",user.UserEmail)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiration = DateTime.UtcNow.AddDays(1);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credential);
            return new TokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationDate = expiration
            };
        }
    }

}
