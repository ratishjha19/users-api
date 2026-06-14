using DemoApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        // Get all users for a specific owner (identity user id)
        Task<List<User>> GetAllByOwnerAsync(string ownerId);

        Task<User?> GetByIdAsync(Guid id);

        // Get a specific user by id only if it belongs to the given owner
        Task<User?> GetByIdAsync(Guid id, string ownerId);

        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(User user);

        Task DeleteAsync(Guid id);
    }
}
