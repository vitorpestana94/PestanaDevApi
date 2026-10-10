namespace PestanaDevApi.Constants.Queries
{
    public static partial class BoardQueries
    {
        public const string DeleteBoard = @"
        START TRANSACTION;

        DELETE FROM
            BOARD_TASKS
        WHERE
            board_id = @BoardId;

        DELETE FROM
            BOARD_COLUMNS
        WHERE
            board_id = @BoardId;

        DELETE FROM
            BOARD_USERS
        WHERE
            board_id = @BoardId;

        DELETE FROM
            BOARD
        WHERE
            id = @BoardId;

        COMMIT;";

        public const string DeleteTask = @"
        DELETE FROM
            BOARD_TASKS
        WHERE
            id = @TaskId
        AND
            board_id = @BoardId;";

        public const string DeleteMember = @"
        START TRANSACTION;

        UPDATE
            BOARD_TASKS
        SET
            task_owner_id = NULL
        WHERE
            board_id = @BoardId
        AND
            task_owner_id = @UserId;

        DELETE FROM
            BOARD_USERS
        WHERE
            board_id = @BoardId
        AND
            user_id = @UserId;

        COMMIT;";
    }
}
