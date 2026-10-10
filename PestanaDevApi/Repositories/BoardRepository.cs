using System.Data;
using Dapper;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.BoardQueries;
using PestanaDevApi.Extensions.Dtos.Requests;

namespace PestanaDevApi.Repositories
{
    public class BoardRepository: IBoardRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertBoard(Guid userId, CreateBoardRequestDto dto)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.Insert, dto.ToInsert(userId));
        }
    }
}
