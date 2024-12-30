using CRUD_Radenta.Data;
using CRUD_Radenta.Model.DTO;
using CRUD_Radenta.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using CRUD_Radenta.Service;
namespace CRUD_Radenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ISendEmail SendEmail;


        public AdminController(ApplicationDbContext dbContext, IConfiguration configuration, ISendEmail SendEmail)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.SendEmail = SendEmail;
        }


        [HttpPost("register")]
        public async Task<IActionResult> AdminRegistration(RegisterAdmin request)
        {
            // Check if the user already exists
            if (await dbContext.Admins.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("User already exists.");
            }

            // Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create a new Admin entity
            var admin = new Admin
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash
            };

            // Save the new admin to the database
            dbContext.Admins.Add(admin);
            await dbContext.SaveChangesAsync();

            return Ok("Admin registered successfully");
        }



        [HttpPost]
        [Route("Login")]
        public IActionResult LoginAdmin(AdminLoginDTO adminLoginDTO)
        {
            var user = dbContext.Admins.FirstOrDefault(x => x.Email == adminLoginDTO.Email && x.Password == adminLoginDTO.Password);

            if(user != null && BCrypt.Net.BCrypt.Verify(adminLoginDTO.Password, user.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Id",  user.Id.ToString()),
                    new Claim("Email" , user.Email.ToString())

                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires:DateTime.UtcNow.AddMinutes(60),
                    signingCredentials:signIn);

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);


                return Ok(new {Token = tokenValue , User = user});

            }

            return NoContent();
        }



        [HttpPost]
        [Route("Email")]
        public async Task<IActionResult>Email(string receptor, string subject, string body)
        {
            await SendEmail.EmailSend(receptor, subject, body);
            return Ok();
        }


        //public static void Sendemail()
        //{
        //    string fromMail = "benjoapolinario@gmail.com";
        //    string fromPassword = "cbzl xmgb mdjb ixop ";

        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress(fromMail);
        //    message.Subject = "Test Subject";
        //    message.To.Add(new MailAddress(fromMail));
        //    message.Body = "<html><body>Test body </body></html>";
        //    message.IsBodyHtml = true;

        //    var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential(fromMail, fromPassword),
        //        EnableSsl = true,

        //    }
            //// Sender email credentials
            //string senderEmail = "benjoapolinario@gmail.com";
            //string senderPassword = "cbzl xmgb mdjb ixop";

            //// Email details
            //string recipientEmail = "recipient@example.com";
            //string subject = "Test Email";
            //string body = "This is a test email sent from ASP.NET.";
            //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.example.com", 587);
            //{
            //    Credentials = new NetworkCredential(senderEmail, senderPassword);
            //    EnableSsl = true;
            //}

            //// Configure the SMTP client


            //try
            //{
            //    // Create the mail message
            //    MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail)
            //    {
            //        Subject = subject,
            //        Body = body,
            //        IsBodyHtml = false // Set to true if the body contains HTML
            //    };

            //    // Send the email
            //    smtpClient.Send(mailMessage);
            //    Console.WriteLine("Email sent successfully.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error sending email: " + ex.Message);
            //}
        

        //public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Your Name", fromEmail));
        //    message.To.Add(new MailboxAddress("Recipient Name", toEmail));
        //    message.Subject = subject;

        //    // Text or HTML body content
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = body
        //    };

        //    using var client = new SmtpClient();
        //    await client.ConnectAsync("admin1@yopmail.com", 587, false);
        //    await client.AuthenticateAsync("benjoapolinario@gmail.com", "cbzl xmgb mdjb ixop"); // Use app password, not your regular password
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}




        //public IActionResult SendEmail()
        //{
        //    EmailSender.SendEmail();
        //    return Content("Email sent successfully.");
        //}

        //    private string CreateJwtToken(Admin admin)
        //    {
        //        var claims = new[]
        //        {
        //    new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //};

        //        // Use _configuration to access JWT settings
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(
        //            issuer: configuration["JWT:Issuer"],
        //            audience: configuration["JWT:Audience"],
        //            claims: claims,
        //            expires: DateTime.Now.AddHours(1),
        //            signingCredentials: creds);

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }



    }
}
