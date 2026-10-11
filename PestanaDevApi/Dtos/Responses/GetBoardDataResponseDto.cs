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
            public IEnumerable<BoardUserData> BoardUsers { get; set; } = [];
            public IEnumerable<BoardColumn> BoardColumns { get; set; } = [];

            public BoardData() { }

            public BoardData((Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns) board)
            {
                Tasks = board.Tasks;
                BoardUsers = board.BoardUsers.Select(user => new BoardUserData(user));
                Board = board.Board;
                BoardColumns = board.BoardColumns;
            }
        }

        public class BoardUserData
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;

            public BoardUserData() { }

            public BoardUserData(User user)
            {
                Id = user.Id;
                Name = user.UserName;
                Email = user.UserEmail;
            }
        }
    }
}
