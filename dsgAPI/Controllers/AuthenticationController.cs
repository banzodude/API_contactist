using dsgAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dsgAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly clientContext _client;
        public AuthenticationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration, clientContext client)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _client = client;
        }

        [Authorize]
        [HttpGet]
        [Route("SearchData")]
        public async Task<IActionResult> SearchData([FromQuery] myparams parms)
        {
          
           var items = await _client.Contacts.FromSqlInterpolated($"returnAPIcontacts {parms.fromDate},{parms.toDate} ").ToListAsync();
            if (items!=null)
            {         
                    //var extUser = await _client.accessRequest.FromSqlInterpolated($"storeAccess {User.Identity.Name} ").ToListAsync();
                    var extUser =_client.Database.ExecuteSqlInterpolated($"storeAccess {User.Identity.Name} ");
                    Ok(items);               
            }
            // var items = await _client.Contacts.ToListAsync();
            return Ok(items);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Userdetails userdetails)
        {
            var user = await _userManager.FindByEmailAsync(userdetails.email);
            if(user!=null && await _userManager.CheckPasswordAsync(user,userdetails.password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                var jtokens = GetToken(authClaims);
                return Ok(new
                {
                    token=new JwtSecurityTokenHandler().WriteToken(jtokens),
                    expiration=jtokens.ValidTo
                }
                    );

            }
           
            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] registerUser newUser)
        {
            
            var user = await _userManager.FindByEmailAsync(newUser.email);
            if (user == null) 
            {
                IdentityUser x = new IdentityUser(newUser.email);
                x.Email = newUser.email;
                await _userManager.AddPasswordAsync(x,newUser.password);
                _ = _userManager.CreateAsync(x);
                //_context.Add(contacts);                                 
             
                return Ok("User created successfully");
            }
            
            return Unauthorized("User already exist");
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(

                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private bool accessExists(string user)
        {
            return _client.accessRequest.Any(e => e.Access == user);
        }
    }
}
