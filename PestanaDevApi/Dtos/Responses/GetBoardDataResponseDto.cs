using System.Net;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Dtos.Responses
{
    public class GetBoardDataResponseDto: DefaultResponseDto
    {
        public BoardData Data { get; set; } = new();

        public GetBoardDataResponseDto((Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns) data)
        {
            Data = new(data);
        }

        public GetBoardDataResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public class BoardData
        {
            public Board Board { get; set; } = new();
            public IEnumerable<BoardTask>? Tasks { get; set; } = [];
            public IEnumerable<User> BoardUsers { get; set; } = [];
            public IEnumerable<BoardColumn> CoardColumns { get; set; } = [];

            public BoardData() { }

            public BoardData((Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns) board)
            {
                Tasks = board.Tasks;
                BoardUsers = board.BoardUsers;
                Board = board.Board;
                CoardColumns = board.BoardColumns;
            }
        }
    }
}
