using ConstructionPM.Application.DTOs;
using ConstructionPM.Application.Interfaces.Repositories.Commands;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Application.Interfaces.Services;
using ConstructionPM.Domain.Entities;
using ConstructionPM.Domain.Enums;


namespace ConstructionPM.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationCommandRepository _command;
        private readonly IRoleQueryRepository _roleQuery;

        public RegistrationService(IRegistrationCommandRepository command,
            IRoleQueryRepository roleQuery)
        {
            _command = command;
            _roleQuery = roleQuery;
        }

        public async Task RegisterAsync(RegistrationRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is required");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required");

            var roleName = request.RoleName.ToString();

            //Console.WriteLine($"Role Name: {roleName}");

            var roleExists = await _roleQuery.GetRoleIdByNameAsync(roleName);

            if (roleExists == null)
                throw new ArgumentException("Invalid role");

            ValidateByRole(request);

            var entity = MapToEntity(request);
            await _command.CreateAsync(entity);
        }

        private static void ValidateByRole(RegistrationRequestDto r)
        {
            switch (r.RoleName)
            {
                case RegistrationRole.ProjectManager:
                    if (r.ExperienceYears is null || r.ExperienceYears <= 0)
                        throw new ArgumentException("ExperienceYears is required for PM");
                    break;

                case RegistrationRole.SiteEngineer:
                    if (r.ExperienceYears is null || r.ExperienceYears <= 0)
                        throw new ArgumentException("ExperienceYears is required for Engineer");
                    if (string.IsNullOrWhiteSpace(r.Skills))
                        throw new ArgumentException("Skills are required for Engineer");
                    break;

                case RegistrationRole.Client:
                    if (string.IsNullOrWhiteSpace(r.ProjectName))
                        throw new ArgumentException("ProjectName is required for Client");
                    break;

                default:
                    throw new ArgumentException("Invalid role");
            }
        }

        private static RegistrationRequest MapToEntity(RegistrationRequestDto r)
        {
            var roleName = r.RoleName.ToString();


            return new RegistrationRequest
            {
                Name = r.Name,
                Email = r.Email,
                Phone = r.PhoneNumber,
                RoleName = roleName,
                ExperienceYears = r.ExperienceYears,
                Skills = r.Skills,
                ProjectName = r.ProjectName,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }
    }
}






































//using ConstructionPM.Application.DTOs.RegistrationDTOs;
//using ConstructionPM.Application.Interfaces.Repositories.Commands;
//using ConstructionPM.Application.Interfaces.Services;
//using ConstructionPM.Domain.Entities;

//namespace ConstructionPM.Application.Services
//{
//    public class RegistrationService : IRegistrationService
//    {
//        private readonly IRegistrationCommandRepository _command;

//        public RegistrationService(IRegistrationCommandRepository command)
//        {
//            _command = command;
//        }

//        public async Task RegisterAsync(RegistrationBaseDto request)
//        {
//            if (string.IsNullOrWhiteSpace(request.RoleName))
//                throw new ArgumentException("Role is required");

//            switch (request.RoleName)
//            {
//                case "PM":
//                    await HandlePMAsync(request);
//                    break;

//                case "Engineer":
//                    await HandleEngineerAsync(request);
//                    break;

//                case "Client":
//                    await HandleClientAsync(request);
//                    break;

//                default:
//                    throw new ArgumentException("Invalid role");
//            }
//        }

//        // =========================
//        // Handlers (Business Logic)
//        // =========================

//        private async Task HandlePMAsync(RegistrationBaseDto baseDto)
//        {
//            if (baseDto is not PMRegistrationDto dto)
//                throw new ArgumentException("Invalid PM registration data");

//            if (dto.ExperienceYears <= 0)
//                throw new ArgumentException("ExperienceYears must be greater than 0");

//            var entity = new RegistrationRequest
//            {
//                Name = dto.Name,
//                Email = dto.Email,
//                RoleName = "PM",
//                ExperienceYears = dto.ExperienceYears
//            };

//            await _command.CreateAsync(entity);
//        }

//        private async Task HandleEngineerAsync(RegistrationBaseDto baseDto)
//        {
//            if (baseDto is not EngineerRegistrationDto dto)
//                throw new ArgumentException("Invalid Engineer registration data");

//            if (dto.ExperienceYears <= 0)
//                throw new ArgumentException("ExperienceYears must be greater than 0");

//            if (string.IsNullOrWhiteSpace(dto.Skills))
//                throw new ArgumentException("Skills are required");

//            var entity = new RegistrationRequest
//            {
//                Name = dto.Name,
//                Email = dto.Email,
//                RoleName = "Engineer",
//                ExperienceYears = dto.ExperienceYears,
//                Skills = dto.Skills
//            };

//            await _command.CreateAsync(entity);
//        }

//        private async Task HandleClientAsync(RegistrationBaseDto baseDto)
//        {
//            if (baseDto is not ClientRegistrationDto dto)
//                throw new ArgumentException("Invalid Client registration data");

//            if (string.IsNullOrWhiteSpace(dto.ProjectName))
//                throw new ArgumentException("ProjectName is required");

//            var entity = new RegistrationRequest
//            {
//                Name = dto.Name,
//                Email = dto.Email,
//                RoleName = "Client",
//                ProjectName = dto.ProjectName
//            };

//            await _command.CreateAsync(entity);
//        }
//    }
//}
