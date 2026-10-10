using System.Data;
using System.Net;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Dtos.Responses
{
    public class GetBoardsResponseDto: DefaultResponseDto
    {
        public IEnumerable<BoardIdsAndNames> Boards { get; set; } = [];

        public GetBoardsResponseDto(IEnumerable<Board> boards) : base()
        {
            Boards = boards.Select(board => new BoardIdsAndNames(board.Id, board.Name));
        }

        public GetBoardsResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }

    public class BoardIdsAndNames
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;


        public BoardIdsAndNames() { }

        public BoardIdsAndNames(Guid id, string name) 
        { 
            Id = id;
            Name = name;
        }
    }
}
