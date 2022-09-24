using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDTO dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authSettings;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authSettings = authSettings;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(u=>u.Role)
                .FirstOrDefault(u => u.Email.Equals(dto.Email));

            if (user is null)
                throw new BadRequestException("Invalid username or password");

            if(_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid username or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyy-MM-dd")),
                new Claim("Nationality", user.Nationality)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(_authSettings.JwtIssuer, 
                _authSettings.JwtIssuer, 
                claims, expires: expires, 
                signingCredentials: 
                credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDTO dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };
            var hashedPass = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPass;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }
    }
}
