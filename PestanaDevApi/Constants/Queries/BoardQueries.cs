namespace PestanaDevApi.Constants.Queries
{
    public static class BoardQueries
    {
        public const string Insert = @"
        START TRANSACTION;

            INSERT INTO BOARD(owner_id, name)
            VALUES(@UserId, @Name);

            SET @new_board_id = LAST_INSERT_ID();

            INSERT INTO BOARD_USERS(board_id, owner_id)
            VALUES(@new_board_id, @UserId);

            INSERT INTO BOARD_COLUMNS(board_id, name, position)
            VALUES(@new_board_id, @ColumnNameOne, 1);

            SET @first_column_id = LAST_INSERT_ID();

            INSERT INTO BOARD_COLUMNS(board_id, name, position)
            VALUES
                (@new_board_id, @ColumnNameTwo, 2),
                (@new_board_id, @ColumnNameThree, 3),
                (@new_board_id, @ColumnNameFour, 4),
                (@new_board_id, @ColumnNameFive, 5);

            INSERT INTO BOARD_TASKS(
                board_id, column_id, task_owner_id,
                task_name, task_type, task_description
            )
            VALUES(
                @new_board_id, @first_column_id, @UserId,
                @TaskName, @TaskType, @TaskDescription
            );

        COMMIT;";
    }
}
