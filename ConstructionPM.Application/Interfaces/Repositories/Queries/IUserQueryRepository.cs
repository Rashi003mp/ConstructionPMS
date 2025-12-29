using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionPM.Application.DTOs;

namespace ConstructionPM.Application.Interfaces.Repositories.Queries
{
    public interface IUserQueryRepository
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
    }
}
