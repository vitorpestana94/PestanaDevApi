using System.Net;
using Moq;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Services;

namespace PestanaDevApi.Tests.Services
{
    /// <summary>
    /// Shared ids and mocks used by all the board services tests.
    /// By default the user is the owner and a member of the board.
    /// </summary>
    public abstract class BoardTestsBase
    {
        protected readonly Guid OwnerId = Guid.NewGuid();
        protected readonly Guid BoardId = Guid.NewGuid();
        protected readonly Mock<IBoardPanelRepository> BoardRepository = new();
        protected readonly Mock<IBoardCacheService> CacheService = new();
        protected readonly Mock<IBoardNotificationService> NotificationService = new();
        protected readonly ISemaphoreService SemaphoreService = new SemaphoreService();

        protected BoardTestsBase()
        {
            BoardRepository.Setup(r => r.GetBoard(BoardId)).ReturnsAsync(new Board { Id = BoardId, OwnerId = OwnerId, Name = "Board" });
            BoardRepository.Setup(r => r.IsBoardMember(BoardId, OwnerId)).ReturnsAsync(true);
        }
    }

    public class BoardServiceTests
    {
        private readonly Mock<IBoardRepository> _repository = new();
        private readonly Mock<IBoardCacheService> _cacheService = new();
        private readonly BoardService _service;

        public BoardServiceTests()
        {
            _service = new(_repository.Object, _cacheService.Object);
        }

        [Fact]
        public async Task GetBoardData_ReturnsCache_WithoutRequestingTheDatabase()
        {
            Guid boardId = Guid.NewGuid();
            GetBoardDataResponseDto cached = new(HttpStatusCode.OK, "cached");
            _cacheService.Setup(c => c.GetBoardDataCache(boardId)).Returns(cached);

            GetBoardDataResponseDto response = await _service.GetBoardData(boardId);

            Assert.Same(cached, response);
            _repository.Verify(r => r.GetBoardData(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task GetBoardData_ReturnsNotFoundAndDoesNotCache_WhenBoardDoesNotExist()
        {
            GetBoardDataResponseDto response = await _service.GetBoardData(Guid.NewGuid());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _cacheService.Verify(c => c.SetBoardDataCache(It.IsAny<Guid>(), It.IsAny<GetBoardDataResponseDto>()), Times.Never);
        }

        [Fact]
        public async Task GetBoardData_CachesTheResponse_WhenBoardExists()
        {
            Guid boardId = Guid.NewGuid();
            _repository.Setup(r => r.GetBoardData(boardId)).ReturnsAsync((new Board(), new List<BoardTask>(), new List<User>(), new List<BoardColumn>()));

            GetBoardDataResponseDto response = await _service.GetBoardData(boardId);

            Assert.True(response.IsSuccess);
            _cacheService.Verify(c => c.SetBoardDataCache(boardId, response), Times.Once);
        }

        [Fact]
        public async Task CreateBoard_ClearsUserBoardsCache()
        {
            Guid userId = Guid.NewGuid();

            await _service.CreateBoard(userId, new());

            _cacheService.Verify(c => c.DeleteUserBoardsCache(userId), Times.Once);
        }
    }

    public class BoardPanelServiceTests: BoardTestsBase
    {
        private readonly BoardPanelService _service;

        public BoardPanelServiceTests()
        {
            _service = new(BoardRepository.Object, CacheService.Object, NotificationService.Object);
        }

        [Fact]
        public async Task UpdateBoard_ReturnsBadRequest_WhenNothingChanged()
        {
            UpdateBoardResponseDto response = await _service.UpdateBoard(OwnerId, BoardId, new());

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBoard_ReturnsForbidden_WhenUserIsNotTheOwner()
        {
            UpdateBoardResponseDto response = await _service.UpdateBoard(Guid.NewGuid(), BoardId, new() { Name = "New" });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            BoardRepository.Verify(r => r.UpdateBoard(It.IsAny<Guid>(), It.IsAny<UpdateBoardRequestDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateBoard_ClearsCacheAndNotifies_WhenUpdated()
        {
            BoardRepository.Setup(r => r.UpdateBoard(BoardId, It.IsAny<UpdateBoardRequestDto>())).ReturnsAsync(true);

            UpdateBoardResponseDto response = await _service.UpdateBoard(OwnerId, BoardId, new() { Name = "New" });

            Assert.True(response.IsSuccess);
            CacheService.Verify(c => c.DeleteBoardDataCache(BoardId), Times.Once);
            NotificationService.Verify(n => n.NotifyBoardUpdated(OwnerId, It.IsAny<Board>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBoard_ReturnsNotFound_WhenBoardDoesNotExist()
        {
            DeleteBoardResponseDto response = await _service.DeleteBoard(OwnerId, Guid.NewGuid());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBoard_Notifies_WhenDeleted()
        {
            BoardRepository.Setup(r => r.DeleteBoard(BoardId)).ReturnsAsync(true);

            DeleteBoardResponseDto response = await _service.DeleteBoard(OwnerId, BoardId);

            Assert.True(response.IsSuccess);
            NotificationService.Verify(n => n.NotifyBoardDeleted(OwnerId, BoardId), Times.Once);
        }
    }

    public class BoardTaskServiceTests: BoardTestsBase
    {
        private readonly Mock<IBoardTaskRepository> _repository = new();
        private readonly Mock<IBoardColumnRepository> _columnRepository = new();
        private readonly BoardTaskService _service;
        private readonly Guid _taskId = Guid.NewGuid();
        private readonly Guid _columnId = Guid.NewGuid();

        public BoardTaskServiceTests()
        {
            _repository.Setup(r => r.GetTask(BoardId, _taskId)).ReturnsAsync(new BoardTask { Id = _taskId, BoardId = BoardId, TaskName = "Task" });
            _columnRepository.Setup(r => r.GetBoardColumns(BoardId)).ReturnsAsync([new BoardColumn { Id = _columnId, BoardId = BoardId, Name = "Todo" }]);
            _service = new(_repository.Object, BoardRepository.Object, _columnRepository.Object, SemaphoreService, CacheService.Object, NotificationService.Object);
        }

        [Fact]
        public async Task UpdateTask_ReturnsForbidden_WhenUserIsNotMember()
        {
            UpdateBoardTaskResponseDto response = await _service.UpdateTask(Guid.NewGuid(), BoardId, _taskId, new() { TaskName = "New" });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            UpdateBoardTaskResponseDto response = await _service.UpdateTask(OwnerId, BoardId, Guid.NewGuid(), new() { TaskName = "New" });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound_WhenMovingToAColumnOfAnotherBoard()
        {
            UpdateBoardTaskResponseDto response = await _service.UpdateTask(OwnerId, BoardId, _taskId, new() { ColumnId = Guid.NewGuid() });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _repository.Verify(r => r.UpdateTask(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdateBoardTaskRequestDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTask_ReturnsBadRequest_WhenNewOwnerIsNotMember()
        {
            UpdateBoardTaskResponseDto response = await _service.UpdateTask(OwnerId, BoardId, _taskId, new() { TaskOwnerId = Guid.NewGuid() });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ClearsCacheAndNotifies_WhenMoved()
        {
            _repository.Setup(r => r.UpdateTask(BoardId, _taskId, It.IsAny<UpdateBoardTaskRequestDto>())).ReturnsAsync(true);

            UpdateBoardTaskResponseDto response = await _service.UpdateTask(OwnerId, BoardId, _taskId, new() { ColumnId = _columnId });

            Assert.True(response.IsSuccess);
            CacheService.Verify(c => c.DeleteBoardDataCache(BoardId), Times.Once);
            NotificationService.Verify(n => n.NotifyTaskUpdated(OwnerId, It.IsAny<BoardTask>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_Notifies_WhenDeleted()
        {
            _repository.Setup(r => r.DeleteTask(BoardId, _taskId)).ReturnsAsync(true);

            DeleteBoardTaskResponseDto response = await _service.DeleteTask(OwnerId, BoardId, _taskId);

            Assert.True(response.IsSuccess);
            NotificationService.Verify(n => n.NotifyTaskDeleted(OwnerId, BoardId, _taskId), Times.Once);
        }
    }

    public class BoardColumnServiceTests: BoardTestsBase
    {
        private readonly Mock<IBoardColumnRepository> _repository = new();
        private readonly BoardColumnService _service;
        private readonly Guid _todoId = Guid.NewGuid();

        public BoardColumnServiceTests()
        {
            _repository.Setup(r => r.GetBoardColumns(BoardId)).ReturnsAsync(
            [
                new BoardColumn { Id = _todoId, BoardId = BoardId, Name = "Todo", Position = 1 },
                new BoardColumn { Id = Guid.NewGuid(), BoardId = BoardId, Name = "Done", Position = 2 }
            ]);
            _service = new(_repository.Object, BoardRepository.Object, SemaphoreService, CacheService.Object, NotificationService.Object);
        }

        [Fact]
        public async Task UpdateColumn_ReturnsNotFound_WhenColumnDoesNotExist()
        {
            UpdateBoardColumnResponseDto response = await _service.UpdateColumn(OwnerId, BoardId, Guid.NewGuid(), new() { Position = 2 });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateColumn_ReturnsBadRequest_WhenNameIsUsedByAnotherColumn()
        {
            UpdateBoardColumnResponseDto response = await _service.UpdateColumn(OwnerId, BoardId, _todoId, new() { Name = "done" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateColumn_ClearsCacheAndNotifiesAllColumns_WhenUpdated()
        {
            _repository.Setup(r => r.UpdateColumn(It.IsAny<UpdateBoardColumnRequestDto>(), It.IsAny<BoardColumn>(), 2)).ReturnsAsync(true);

            UpdateBoardColumnResponseDto response = await _service.UpdateColumn(OwnerId, BoardId, _todoId, new() { Position = 2 });

            Assert.True(response.IsSuccess);
            CacheService.Verify(c => c.DeleteBoardDataCache(BoardId), Times.Once);
            NotificationService.Verify(n => n.NotifyColumnsUpdated(OwnerId, BoardId, It.Is<IEnumerable<BoardColumn>>(c => c.Count() == 2)), Times.Once);
        }
    }

    public class BoardMemberServiceTests: BoardTestsBase
    {
        private readonly Mock<IBoardMemberRepository> _repository = new();
        private readonly BoardMemberService _service;
        private readonly User _newMember = new() { Id = Guid.NewGuid(), UserName = "New", UserEmail = "new@email.com" };

        public BoardMemberServiceTests()
        {
            _repository.Setup(r => r.GetUserByEmail(_newMember.UserEmail)).ReturnsAsync(_newMember);
            _service = new(_repository.Object, BoardRepository.Object, SemaphoreService, CacheService.Object, NotificationService.Object);
        }

        [Fact]
        public async Task GetBoardMembers_ReturnsForbidden_WhenUserIsNotMember()
        {
            GetBoardMembersResponseDto response = await _service.GetBoardMembers(Guid.NewGuid(), BoardId);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AddBoardMember_ReturnsForbidden_WhenUserIsNotTheOwner()
        {
            AddBoardMemberResponseDto response = await _service.AddBoardMember(Guid.NewGuid(), BoardId, new() { Email = _newMember.UserEmail });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AddBoardMember_ReturnsNotFound_WhenEmailIsNotRegistered()
        {
            AddBoardMemberResponseDto response = await _service.AddBoardMember(OwnerId, BoardId, new() { Email = "unknown@email.com" });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddBoardMember_ReturnsBadRequest_WhenUserIsAlreadyMember()
        {
            BoardRepository.Setup(r => r.IsBoardMember(BoardId, _newMember.Id)).ReturnsAsync(true);

            AddBoardMemberResponseDto response = await _service.AddBoardMember(OwnerId, BoardId, new() { Email = _newMember.UserEmail });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddBoardMember_ClearsCacheAndNotifies_WhenAdded()
        {
            _repository.Setup(r => r.InsertMember(BoardId, _newMember.Id)).ReturnsAsync(true);

            AddBoardMemberResponseDto response = await _service.AddBoardMember(OwnerId, BoardId, new() { Email = _newMember.UserEmail });

            Assert.True(response.IsSuccess);
            CacheService.Verify(c => c.DeleteBoardMembersCache(BoardId), Times.Once);
            NotificationService.Verify(n => n.NotifyMemberAdded(OwnerId, BoardId, It.Is<BoardMember>(m => m.Id == _newMember.Id && !m.IsOwner)), Times.Once);
        }

        [Fact]
        public async Task RemoveBoardMember_ReturnsBadRequest_WhenRemovingTheOwner()
        {
            RemoveBoardMemberResponseDto response = await _service.RemoveBoardMember(OwnerId, BoardId, OwnerId);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveBoardMember_ReturnsForbidden_WhenMemberRemovesAnotherMember()
        {
            RemoveBoardMemberResponseDto response = await _service.RemoveBoardMember(Guid.NewGuid(), BoardId, _newMember.Id);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task RemoveBoardMember_AllowsMemberToLeaveTheBoard()
        {
            BoardRepository.Setup(r => r.IsBoardMember(BoardId, _newMember.Id)).ReturnsAsync(true);
            _repository.Setup(r => r.DeleteMember(BoardId, _newMember.Id)).ReturnsAsync(true);

            RemoveBoardMemberResponseDto response = await _service.RemoveBoardMember(_newMember.Id, BoardId, _newMember.Id);

            Assert.True(response.IsSuccess);
            NotificationService.Verify(n => n.NotifyMemberRemoved(_newMember.Id, BoardId, _newMember.Id), Times.Once);
        }
    }
}
