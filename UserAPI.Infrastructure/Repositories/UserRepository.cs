using DemoApp.Domain.Entities;
using DemoApp.Domain.Interfaces;
using DemoApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllByOwnerAsync(string ownerId)
        {
            return await _context.Users
                .Where(x => x.OwnerId == ownerId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByIdAsync(Guid id, string ownerId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == ownerId);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user =
                await _context.Users.FindAsync(id);

            if (user == null)
                return;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }
    }
}
