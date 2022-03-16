using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MYSQLStoreAPI.Authentication;
using MYSQLStoreAPI.Models;

namespace MYSQLStoreAPI.Controllers
{   
   // [ApiExplorerSettings(IgnoreApi = true)] //ซ่อน path
   [Route("api/[controller]")]
   [ApiController]

    public class AuthenticateController:ControllerBase

    {
         // ประกอบด้วย property 3 อย่างที่สำคัญ คือ

         // ประกอบด้วย property 3 อย่างที่สำคัญ คือ

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        
        // ทำ login API
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) //เขียนแบบ async
        {
            var user = await userManager.FindByNameAsync(model.Username); //ดึงหาแค่ username ที่ผู้ใช้ส่งมาจาก FromBody
            
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) //เช็คว่า password ต้องถูกต้อง
            {
                var userRoles = await userManager.GetRolesAsync(user); // ดึง Role ที่ตรงกับ user มาเก็บไว้ที่ userRoles
                var authClaims = new List<Claim> // ประกาศ authClaims 
                {
                    new Claim(ClaimTypes.Name, user.UserName), // เปรียบเทียบ username กับ Password โดยใช้ Claim
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // สร้าง Jwt ในการ gen token
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole)); // userที่ login เข้ามามี Role อะไรบ้าง
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(5), // วันหมดอายุ AddDay,Minute ได้
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                }); 
            }
            return Unauthorized();
        }

        // Register API สำหรับ user
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                    Status = "Error", Message = "User already exists!" 
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response {
                     Status = "Error", Message = "User creation failed! Please check user details and try again." 
                });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        // Register for Admin API
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                    Status = "Error", Message = "User already exists!" 
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                    Status = "Error", Message = "User creation failed! Please check user details and try again." 
                });
            }

            // กำหนดสิทธิ์เป็น admin

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin)){
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.User)){
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


    }

}
