using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDTO dto);
    }
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
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
