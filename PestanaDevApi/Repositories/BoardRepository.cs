using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;

namespace PestanaDevApi.Repositories
{
    public class BoardRepository: IBoardRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

    }
}
