using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionPM.Application.DTOs;

namespace ConstructionPM.Application.Interfaces.Repositories.Queries
{
    internal interface IProjectQueryRepository
    {
        Task<IEnumerable<ProjectListDto>> GetAllAsync();
        Task<ProjectDetailsDto?> GetByIdAsync(int projectId);
    }
}
