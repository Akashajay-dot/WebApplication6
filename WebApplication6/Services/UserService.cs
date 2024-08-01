using Google;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public class UserService
    {
        private readonly AppdbContext _dbContext;

        public UserService(AppdbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetAllUserEmailAddressesAsync()
        {
            return await _dbContext.Users
                .Select(u => u.Email)
                .ToListAsync();
        }
    }
}