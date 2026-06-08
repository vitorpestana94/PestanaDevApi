using System.Data;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.UserQueries;
using Params = PestanaDevApi.Utils.DapperParams;
using PestanaDevApi.Models;
using Dapper;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Extensions.Dtos.Requests;

namespace PestanaDevApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IDbConnectionFactory _factory;

        public UserRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<User?> GetUser(Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<User?>(Sql.GetUser, Params.ToUserId(userId));
        }

        public async Task<bool> UpdateUserData(ChangeUserDataRequestDto dto, User oldData, Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            try
            {
                return await db.ExecuteAsync(Sql.UpdateUser, dto.ToUpdate(currentUserData: oldData, userId)) > 0;
            }
            catch (Exception) 
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserData(Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            try
            {
                return await db.ExecuteAsync(Sql.DeleteUser, Params.ToUserId(userId)) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
