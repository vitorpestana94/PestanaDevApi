using System.Net;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Extensions.Dtos.Requests;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;


namespace PestanaDevApi.Services
{
    public class BoardService: IBoardService
    {
        private readonly IBoardRepository _repository;

        public BoardService(IBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto)
        {

            await _repository.InsertBoard(userId, dto);

            return new();
        }

        public async Task<GetBoardsResponseDto>  GetUserBoardsIds(Guid userId)
        {
            IEnumerable<Board>? boards = await _repository.GetUserBoardsIds(userId);

            if (boards == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.NotFound);

            return new GetBoardsResponseDto(boards);
        }

        public async Task<GetBoardDataResponseDto> GetBoardData(Guid boardId)
        {
            (Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns)? board = await _repository.GetBoardData(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.NotFound);

            return new(board.Value);
        }

        #region Board
        public async Task<UpdateBoardResponseDto> UpdateBoard(Guid userId, Guid boardId, UpdateBoardRequestDto dto)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessage.BoardRequestDontHaveAnyChangedData);

            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            if (!await _repository.UpdateBoard(boardId, dto))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.BoardNotUpdated);

            return new();
        }

        public async Task<DeleteBoardResponseDto> DeleteBoard(Guid userId, Guid boardId)
        {
            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            if (!await _repository.DeleteBoard(boardId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.BoardNotDeleted);

            return new();
        }
        #endregion

        #region Tasks
        public async Task<UpdateBoardTaskResponseDto> UpdateTask(Guid userId, Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessage.TaskRequestDontHaveAnyChangedData);

            if (!await _repository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            BoardTask? task = await _repository.GetTask(boardId, taskId);

            if (task == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.TaskNotFound);

            if (dto.IsMovingTask())
            {
                IEnumerable<BoardColumn> columns = await _repository.GetBoardColumns(boardId);

                if (columns.GetColumn(dto.ColumnId!.Value) == null)
                    return new(HttpStatusCode.NotFound, ErrorMessage.ColumnNotFound);
            }

            if (dto.IsChangingTaskOwner() && !await _repository.IsBoardMember(boardId, dto.TaskOwnerId!.Value))
                return new(ErrorMessage.TaskOwnerIsNotBoardMember);

            if (!await _repository.UpdateTask(boardId, taskId, dto))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.TaskNotUpdated);

            return new();
        }

        public async Task<DeleteBoardTaskResponseDto> DeleteTask(Guid userId, Guid boardId, Guid taskId)
        {
            if (!await _repository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            BoardTask? task = await _repository.GetTask(boardId, taskId);

            if (task == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.TaskNotFound);

            if (!await _repository.DeleteTask(boardId, taskId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.TaskNotDeleted);

            return new();
        }
        #endregion

        #region Columns
        public async Task<CreateBoardColumnResponseDto> CreateColumn(Guid userId, Guid boardId, CreateBoardColumnRequestDto dto)
        {
            if (!await _repository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            IEnumerable<BoardColumn> columns = await _repository.GetBoardColumns(boardId);

            if (columns.HasColumnName(dto.Name))
                return new(ErrorMessage.ColumnNameAlreadyExists);

            BoardColumn? column = await _repository.InsertColumn(boardId, dto);

            if (column == null)
                return new(HttpStatusCode.InternalServerError, ErrorMessage.ColumnNotCreated);

            return new(column);
        }

        public async Task<UpdateBoardColumnResponseDto> UpdateColumn(Guid userId, Guid boardId, Guid columnId, UpdateBoardColumnRequestDto dto)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessage.ColumnRequestDontHaveAnyChangedData);

            if (!await _repository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            IEnumerable<BoardColumn> columns = await _repository.GetBoardColumns(boardId);
            BoardColumn? column = columns.GetColumn(columnId);

            if (column == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.ColumnNotFound);

            if (dto.ColumnNameWasUpdated(currentColumnName: column.Name) && columns.HasColumnName(dto.Name!))
                return new(ErrorMessage.ColumnNameAlreadyExists);

            if (!await _repository.UpdateColumn(dto, column, columns.Count()))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.ColumnNotUpdated);

            return new();
        }

        public async Task<DeleteBoardColumnResponseDto> DeleteColumn(Guid userId, Guid boardId, Guid columnId)
        {
            if (!await _repository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            IEnumerable<BoardColumn> columns = await _repository.GetBoardColumns(boardId);

            if (columns.GetColumn(columnId) == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.ColumnNotFound);

            if (columns.Count() <= 1)
                return new(ErrorMessage.BoardMustHaveAtLeastOneColumn);

            if (!await _repository.DeleteColumn(boardId, columnId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.ColumnNotDeleted);

            return new();
        }
        #endregion

        #region Members
        public async Task<GetBoardMembersResponseDto> GetBoardMembers(Guid userId, Guid boardId)
        {
            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!await _repository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            IEnumerable<User> members = await _repository.GetBoardMembers(boardId);

            return new(members, board);
        }

        public async Task<AddBoardMemberResponseDto> AddBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto)
        {
            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            User? user = await _repository.GetUserByEmail(dto.Email);

            if (user == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.UserNotFound);

            if (await _repository.IsBoardMember(boardId, user.Id))
                return new(ErrorMessage.UserIsAlreadyBoardMember);

            if (!await _repository.InsertMember(boardId, user.Id))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.MemberNotAdded);

            return new();
        }

        public async Task<RemoveBoardMemberResponseDto> RemoveBoardMember(Guid userId, Guid boardId, Guid memberId)
        {
            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId) && userId != memberId)
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            if (board.IsBoardOwner(memberId))
                return new(ErrorMessage.BoardOwnerCannotBeRemoved);

            if (!await _repository.IsBoardMember(boardId, memberId))
                return new(HttpStatusCode.NotFound, ErrorMessage.MemberNotFound);

            if (!await _repository.DeleteMember(boardId, memberId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.MemberNotRemoved);

            return new();
        }
        #endregion

    }
}
