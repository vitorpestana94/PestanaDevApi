using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class CreateBoardTaskRequestDtoExtensions
    {
        public static object ToInsert(this CreateBoardTaskRequestDto dto, Guid taskId, Guid boardId, Guid userId) =>
        new
        {
            TaskId = taskId,
            BoardId = boardId,
            dto.ColumnId,
            TaskOwnerId = dto.GetTaskOwnerId(userId),
            dto.TaskName,
            dto.TaskType,
            dto.TaskDescription
        };

        public static Guid GetTaskOwnerId(this CreateBoardTaskRequestDto dto, Guid userId) => dto.TaskOwnerId ?? userId;
    }
}
