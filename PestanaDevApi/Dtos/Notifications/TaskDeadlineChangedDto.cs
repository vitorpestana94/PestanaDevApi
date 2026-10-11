namespace PestanaDevApi.Dtos.Notifications
{
    /// <summary>
    /// Data of the SignalR event sent when the deadline of a task changes (set, changed or removed).
    /// </summary>
    public class TaskDeadlineChangedDto
    {
        public Guid TaskId { get; set; } = Guid.Empty;
        public string TaskName { get; set; } = string.Empty;
        public DateTime? OldDeadline { get; set; }
        public DateTime? NewDeadline { get; set; }

        public TaskDeadlineChangedDto() { }

        public TaskDeadlineChangedDto(Guid taskId, string taskName, DateTime? oldDeadline, DateTime? newDeadline)
        {
            TaskId = taskId;
            TaskName = taskName;
            OldDeadline = oldDeadline;
            NewDeadline = newDeadline;
        }
    }
}
