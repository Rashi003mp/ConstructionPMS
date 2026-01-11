using ConstructionPM.Application.Interfaces.Auth;
using ConstructionPM.Application.Interfaces.Repositories.Commands;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Application.Interfaces.Services;
using ConstructionPM.Application.Utilities;
using ConstructionPM.Domain.Entities;

namespace ConstructionPM.Application.Services
{
    public class AdminApprovalService : IAdminApprovalService
    {
        private readonly IRegistrationQueryRepository _registrationQuery;
        private readonly IRegistrationCommandRepository _registrationCommand;
        private readonly IUserCommandRepository _userCommand;
        private readonly IRoleQueryRepository _roleQuery;
        private readonly IPasswordService _passwordservice;
        private readonly IEmailService _emailService;

        public AdminApprovalService(
            IRegistrationQueryRepository registrationQuery,
            IRegistrationCommandRepository registrationCommand,
            IUserCommandRepository userCommand,
            IRoleQueryRepository roleQuery,
            IPasswordService PasswordService,
            IEmailService emailService)
        {
            _registrationQuery = registrationQuery;
            _registrationCommand = registrationCommand;
            _userCommand = userCommand;
            _roleQuery = roleQuery;
            _passwordservice = PasswordService;
            _emailService = emailService;
        }

        public async Task ApproveAsync(int requestId)
        {
            var request = await _registrationQuery.GetByIdAsync(requestId);
            Console.WriteLine(request);
            if (request == null || request.Status != "Pending")
                throw new InvalidOperationException("Invalid registration request");
          

            var roleId = await _roleQuery.GetRoleIdByNameAsync(request.RoleName);
            if (roleId == null)
                throw new InvalidOperationException("Role not found");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                RoleId = roleId.Value,
                Phone= request.Phone
            };

            // Temporary password strategy (admin can later reset)
            var tempPassword = PasswordGenerator.GenerateTempPassword();

            user.PasswordHash = _passwordservice.HashPassword(tempPassword);

            await _userCommand.CreateAsync(user);

            request.Status = "Approved";
            await _registrationCommand.UpdateAsync(request);

            await _emailService.SendApprovalEmailAsync(
                user.Email,
                user.Name,
                tempPassword
    );
        }

        public async Task RejectAsync(int requestId)
        {
            var request = await _registrationQuery.GetByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
                throw new InvalidOperationException("Invalid registration request");
            request.Status = "Rejected";
            await _registrationCommand.UpdateAsync(request);
        }

    }
}
