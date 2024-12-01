using CRUD_Radenta.Data;
using CRUD_Radenta.Model.DTO;
using CRUD_Radenta.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Radenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;


        public AdminController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> AdminRegistration(RegisterAdmin request)
        {
            if(await dbContext.Admins.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("User already exist.");
            }

            using var hmac = new HMACSHA256();
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));


            var admin = new Admin
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
            };



            dbContext.Admins.Add(admin);
            await dbContext.SaveChangesAsync();



            return Ok("Admin registered successfully");
        }







        private string CreateJwtToken(Admin admin)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            // Use _configuration to access JWT settings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
