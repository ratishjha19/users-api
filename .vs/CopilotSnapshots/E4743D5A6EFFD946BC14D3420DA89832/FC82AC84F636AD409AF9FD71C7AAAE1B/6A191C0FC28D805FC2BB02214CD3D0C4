using DemoApp.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllAsync();

        Task<UserResponseDto?> GetByIdAsync(Guid id);

        Task<UserResponseDto> CreateAsync(CreateUserDto dto);

        Task<UserResponseDto?> UpdateAsync(
            Guid id,
            UpdateUserDto dto);

        Task<bool> DeleteAsync(Guid id);
    }
}

