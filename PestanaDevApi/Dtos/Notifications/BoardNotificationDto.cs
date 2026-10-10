namespace PestanaDevApi.Dtos.Notifications
{
    /// <summary>
    /// Envelope sent to the board members through SignalR whenever something changes on the board.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the changed data.
    /// </typeparam>
    public class BoardNotificationDto<T>
    {
        public Guid BoardId { get; set; } = Guid.Empty;

        /// <summary>
        /// The user who made the change. The client can use it to ignore its own changes.
        /// </summary>
        public Guid ChangedBy { get; set; } = Guid.Empty;

        public T Data { get; set; } = default!;

        public BoardNotificationDto() { }

        public BoardNotificationDto(Guid boardId, Guid changedBy, T data)
        {
            BoardId = boardId;
            ChangedBy = changedBy;
            Data = data;
        }
    }
}
