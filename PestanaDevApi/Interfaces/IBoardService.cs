using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardService
    {
        Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto);
        Task<GetBoardsResponseDto> GetUserBoardsIds(Guid userId);
        Task<GetBoardDataResponseDto> GetBoardData(Guid boardId);
        Task<UpdateBoardResponseDto> UpdateBoard(Guid userId, Guid boardId, UpdateBoardRequestDto dto);
        Task<DeleteBoardResponseDto> DeleteBoard(Guid userId, Guid boardId);
        Task<UpdateBoardTaskResponseDto> UpdateTask(Guid userId, Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto);
        Task<DeleteBoardTaskResponseDto> DeleteTask(Guid userId, Guid boardId, Guid taskId);
        Task<CreateBoardColumnResponseDto> CreateColumn(Guid userId, Guid boardId, CreateBoardColumnRequestDto dto);
        Task<UpdateBoardColumnResponseDto> UpdateColumn(Guid userId, Guid boardId, Guid columnId, UpdateBoardColumnRequestDto dto);
        Task<DeleteBoardColumnResponseDto> DeleteColumn(Guid userId, Guid boardId, Guid columnId);
        Task<GetBoardMembersResponseDto> GetBoardMembers(Guid userId, Guid boardId);
        Task<AddBoardMemberResponseDto> AddBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto);
        Task<RemoveBoardMemberResponseDto> RemoveBoardMember(Guid userId, Guid boardId, Guid memberId);
    }
}
