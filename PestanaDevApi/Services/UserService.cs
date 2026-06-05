using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;

namespace PestanaDevApi.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUserResponseDto> GetUser(Guid userId)
        {
            User? user = await _repository.GetUser(userId);

            if (user == null)
                return new(ErrorMessages.NotFound);

            return new(user);
        }
    }
}
