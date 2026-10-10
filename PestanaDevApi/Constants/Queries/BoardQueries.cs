namespace PestanaDevApi.Constants.Queries
{
    public static class BoardQueries
    {
        public const string Insert = @"
        START TRANSACTION;

        SET @new_board_id = UUID();
        SET @first_column_id = UUID();

        INSERT INTO BOARD(id, owner_id, name)
        VALUES(@new_board_id, @UserId, @Name);

        INSERT INTO BOARD_USERS(board_id, user_id)
        VALUES(@new_board_id, @UserId);

        INSERT INTO BOARD_COLUMNS(id, board_id, name, position)
        VALUES(@first_column_id, @new_board_id, @ColumnNameOne, 1);

        INSERT INTO BOARD_COLUMNS(id, board_id, name, position)
        VALUES
            (UUID(), @new_board_id, @ColumnNameTwo, 2),
            (UUID(), @new_board_id, @ColumnNameThree, 3),
            (UUID(), @new_board_id, @ColumnNameFour, 4),
            (UUID(), @new_board_id, @ColumnNameFive, 5);

        INSERT INTO BOARD_TASKS(
            id, board_id, column_id, task_owner_id,
            task_name, task_type, task_description
        )
        VALUES(
            UUID(), @new_board_id, @first_column_id, @UserId,
            @TaskName, @TaskType, @TaskDescription
        );

        COMMIT;";
    }
}
