using ConstructionPM.Application.DTOs.Admin;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Domain.Entities;
using ConstructionPM.Infrastructure.Dapper;
using Dapper;

namespace ConstructionPM.Infrastructure.Repositories.Quaries
{
    public class RegistrationQueryRepository : IRegistrationQueryRepository
    {
        private readonly DapperContext _context;

        public RegistrationQueryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegistrationRequestListDto>> GetPendingAsync()
        {
            const string sql = """
            SELECT Id, Name, Email, RoleName, CreatedAt
            FROM RegistrationRequests
            WHERE Status = 'Pending'
              AND IsDeleted = 0
            ORDER BY CreatedAt
        """;

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<RegistrationRequestListDto>(sql);
        }

        public async Task<RegistrationRequest?> GetByIdAsync(int id)
        {
            const string sql = """
            SELECT *
            FROM RegistrationRequests
            WHERE Id = @Id
              AND IsDeleted = 0
        """;

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<RegistrationRequest>(
                sql,
                new { Id = id }
            );
        }
    }

}
