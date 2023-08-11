using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PFApp.Contexts;
using PFApp.Models;
using Microsoft.Extensions.Logging;
using PFApp.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Data;
//using System.Data.SqlClient;


namespace PFApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UsersContext _dbContext;

        public UsersController(UserManager<User> userManager,SignInManager<User> signInManager,IConfiguration configuration, UsersContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO model){

            var user = new User{
              Name = model.Name,
              UserName = model.Name,
              Email = model.Email,
              Income = model.Income, 
            };

            var result = await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded){
                return StatusCode(201);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO model){

            var user = await _userManager.FindByNameAsync(model.UserName);
            
            if(user == null){
                return BadRequest(new {message = "User is not found"});
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
        
            if(result.Succeeded){
                return Ok(new{
                    token = GenerateJwtToken(user)
                });
            }
            return Unauthorized();
        
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Appsettings:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor{
                
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string user_id)
        {
            var user = await _userManager.FindByIdAsync(user_id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(JsonElement data ){

        string Id = data.GetProperty("Id").ToString();
        string name = data.GetProperty("Name").ToString();
        string email = data.GetProperty("Email").ToString();
        string userName = data.GetProperty("UserName").ToString();
        int income = Convert.ToInt32(data.GetProperty("Income").ToString());
        string phoneNumber = data.GetProperty("PhoneNumber").ToString();
        
        var user = await _userManager.FindByIdAsync(Id);
         
        if (user == null)
        {
            return NotFound();
        } 
        var parameters = new[]
        {
            new SqlParameter("@Id", SqlDbType.Int) { Value = Id },
            new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name },
            new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email },
            new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = userName },
            new SqlParameter("@Income", SqlDbType.Int) { Value = income },
            new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = phoneNumber }
        };

        await _dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateUser @Id, @Name, @Email, @UserName, @Income, @PhoneNumber", parameters);

        return NoContent();
        }

    }
}