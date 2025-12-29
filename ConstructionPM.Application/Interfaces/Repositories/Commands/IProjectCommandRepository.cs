using ConstructionPM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionPM.Application.Interfaces.Repositories.Commands
{
    internal interface IProjectCommandRepository
    {
        Task<int> CreateAsync(Project project);
        Task UpdateStatusAsync(int projectId, int newStatus, int changedByUserId);
    }
}
