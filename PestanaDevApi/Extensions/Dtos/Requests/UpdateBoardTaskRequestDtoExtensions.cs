using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class UpdateBoardTaskRequestDtoExtensions
    {
        public static object ToUpdate(this UpdateBoardTaskRequestDto dto, Guid boardId, Guid taskId) =>
        new
        {
            BoardId = boardId,
            TaskId = taskId,
            dto.ColumnId,
            dto.TaskOwnerId,
            TaskName = string.IsNullOrEmpty(dto.TaskName) ? null : dto.TaskName,
            dto.TaskType,
            dto.TaskDescription
        };

        public static bool WasDataNotUpdated(this UpdateBoardTaskRequestDto dto) =>
            dto.ColumnId == null &&
            dto.TaskOwnerId == null &&
            string.IsNullOrEmpty(dto.TaskName) &&
            dto.TaskType == null &&
            dto.TaskDescription == null;

        public static bool IsMovingTask(this UpdateBoardTaskRequestDto dto) => dto.ColumnId != null;

        public static bool IsChangingTaskOwner(this UpdateBoardTaskRequestDto dto) => dto.TaskOwnerId != null;
    }
}
