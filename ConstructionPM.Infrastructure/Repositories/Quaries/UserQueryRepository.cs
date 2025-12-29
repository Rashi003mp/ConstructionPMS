using ConstructionPM.Application.DTOs;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Infrastructure.Dapper;
using Dapper;


namespace ConstructionPM.Infrastructure.Repositories.Quaries
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly DapperContext _context;

        public UserQueryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var sql = """
            SELECT u.Id, u.Name, u.Email, r.RoleName
            FROM Users u
            JOIN Roles r ON u.RoleId = r.Id
            WHERE u.Email = @Email
        """;

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { Email = email });
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var sql = """
            SELECT u.Id, u.Name, u.Email, r.RoleName
            FROM Users u
            JOIN Roles r ON u.RoleId = r.Id
            WHERE u.Id = @Id
        """;

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { Id = id });
        }
    }
}
