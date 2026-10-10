using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;

namespace PestanaDevApi.Services
{
    public class BoardService: IBoardService
    {
        private readonly IBoardRepository _repository;

        public BoardService(IBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateBoard(Guid userId)
        {

        }
    }
}
