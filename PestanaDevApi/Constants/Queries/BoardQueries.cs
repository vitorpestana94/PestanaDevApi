namespace PestanaDevApi.Constants.Queries
{
    public static class BoardQueries
    {
        #region Inserts
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
        #endregion

        #region Selects
        public const string Select = @"
        SELECT
            id AS Id,
            owner_id AS OwnerId,
            name AS Name,
            description AS Description,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM BOARD
        WHERE owner_id = @UserId;";

        public const string SelectBoardData = @"
        SELECT
            id AS Id,
            owner_id AS OwnerId,
            name AS Name,
            description AS Description,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM BOARD
        WHERE id = @Id;

        SELECT
            id AS Id,
            board_id AS BoardId,
            name AS Name,
            position AS Position,
            created_at AS CreatedAt
        FROM BOARD_COLUMNS
        WHERE board_id = @Id;

        SELECT
            id AS Id,
            board_id AS BoardId,
            column_id AS ColumnId,
            task_owner_id AS TaskOwnerId,
            task_type AS TaskType,
            task_name AS TaskName,
            task_description AS TaskDescription,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM BOARD_TASKS
        WHERE board_id = @Id;

        SELECT
            u.id AS Id,
            u.user_name AS UserName,
            u.user_email AS UserEmail
        FROM USERS_PROFILE_DATA u
        INNER JOIN BOARD_USERS bu ON bu.user_id = u.id
        WHERE bu.board_id = @Id;";
        #endregion
    }
}
