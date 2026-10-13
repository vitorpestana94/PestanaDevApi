namespace PestanaDevApi.Constants.Queries
{
    public static partial class BoardQueries
    {
        public const string UpdateBoard = @"
        UPDATE
            BOARD
        SET
            name = COALESCE(@Name, name),
            description = COALESCE(@Description, description)
        WHERE
            id = @BoardId;";

        public const string UpdateTask = @"
        UPDATE
            BOARD_TASKS
        SET
            column_id = COALESCE(@ColumnId, column_id),
            task_owner_id = COALESCE(@TaskOwnerId, task_owner_id),
            task_type = COALESCE(@TaskType, task_type),
            task_name = COALESCE(@TaskName, task_name),
            task_description = COALESCE(@TaskDescription, task_description),
            deadline = IF(@ClearDeadline, NULL, COALESCE(@Deadline, deadline))
        WHERE
            id = @TaskId
        AND
            board_id = @BoardId;";

        // This update is more thoroughly documented because it is more complex, and I felt the additional documentation was necessary.
        /// Renames and/or moves a column to a new position, shifting the columns in between.
        /// 
        /// Parameters: @BoardId, @ColumnId, @Name (null keeps the current name),
        /// @OldPosition (current position) and @NewPosition (target position, or the current one if it is not moving).
        /// 
        /// Why the +10000 offset: the constraint UNIQUE (board_id, position) is checked row by row
        /// during the UPDATE, so a simple "position + 1" would collide with the next column halfway through.
        /// A negative temporary value is also not allowed because of CHECK (position >= 0).
        /// 
        /// Moving D from 4 to 1 (A=1, B=2, C=3, D=4):
        /// 
        /// step 1 → A, B, C go to 10001, 10002, 10003;
        /// step 2 → D goes to 1;
        /// step 3 → A, B, C come back as 2, 3, 4.
  
        public const string UpdateColumn = @"
        START TRANSACTION;

        UPDATE
            BOARD_COLUMNS
        SET
            position = position + 10000
        WHERE
            board_id = @BoardId
        AND
            id <> @ColumnId
        AND
            position BETWEEN LEAST(@OldPosition, @NewPosition) AND GREATEST(@OldPosition, @NewPosition);

        UPDATE
            BOARD_COLUMNS
        SET
            name = COALESCE(@Name, name),
            position = @NewPosition
        WHERE
            id = @ColumnId
        AND
            board_id = @BoardId;

        UPDATE
            BOARD_COLUMNS
        SET
            position = position - 10000 + IF(@NewPosition < @OldPosition, 1, -1)
        WHERE
            board_id = @BoardId
        AND
            position >= 10000;

        COMMIT;";
    }
}
