using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalR_test.DTOs;
using SignalR_test.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalR_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;

        public RegisterController(AppDbContext context, UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthModel { Massage = "Email is already registered!" };
            }
            if (await _userManager.FindByNameAsync(model.Username) != null)
            {
                return new AuthModel { Massage = "Username is already registered!" };
            }
            var data = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(data, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {

                    errors += $"{error.Description} ,";
                }
                return new AuthModel { Massage = errors };
            }
            await _userManager.AddToRoleAsync(data, "User");
            var jwtSecurityToken = await CreateJwtToken(data);
            return new AuthModel
            {
                Email = data.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Username = data.UserName,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }


        public async Task<string> AddToRoleAsync(AddToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var role = await _roleManager.RoleExistsAsync(model.Role);
            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            {
                return "UserId or role is not valid!";
            }
            if (await _userManager.IsInRoleAsync(user, model.Role))
            {
                return "User is already assigned to this role!";
            }
            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Somthing went wrong please try again !";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }




    }
}


//public async Task<AuthModel> GetJwtToken(LoginDTO model)
//{
//    var authModel = new AuthModel();
//    var user = await _userManager.FindByNameAsync(model.Username);
//    var pass = await _userManager.CheckPasswordAsync(user, model.Password);
//    if (user is null || !pass)
//    {
//        authModel.Massage = "Username Or Passwsord is incorrect CHECK YOU CREDENTIALS!";
//        return authModel;
//    }
//    var jwtSecurityToken = await CreateJwtToken(user);
//    var rolesList = await _userManager.GetRolesAsync(user);
//    authModel.Username = user.UserName;
//    authModel.Roles = rolesList.ToList();
//    authModel.IsAuthenticated = true;
//    authModel.Email = user.Email;
//    authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//    authModel.ExpiresOn = jwtSecurityToken.ValidTo;
//    return authModel;
//}