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

        public const string InsertColumn = @"
        INSERT INTO BOARD_COLUMNS(id, board_id, name, position)
        SELECT
            @ColumnId, @BoardId, @Name, COALESCE(MAX(position), 0) + 1
        FROM BOARD_COLUMNS
        WHERE board_id = @BoardId;

        SELECT
            id AS Id,
            board_id AS BoardId,
            name AS Name,
            position AS Position,
            created_at AS CreatedAt
        FROM BOARD_COLUMNS
        WHERE id = @ColumnId;";

        public const string InsertMember = @"
        INSERT INTO BOARD_USERS(board_id, user_id)
        VALUES(@BoardId, @UserId);";
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

        public const string SelectBoardById = @"
        SELECT
            id AS Id,
            owner_id AS OwnerId,
            name AS Name,
            description AS Description,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM BOARD
        WHERE id = @BoardId;";

        public const string SelectIsBoardMember = @"
        SELECT EXISTS(
            SELECT 1
            FROM BOARD_USERS
            WHERE board_id = @BoardId
            AND user_id = @UserId
        );";

        public const string SelectTask = @"
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
        WHERE id = @TaskId
        AND board_id = @BoardId;";

        public const string SelectBoardColumns = @"
        SELECT
            id AS Id,
            board_id AS BoardId,
            name AS Name,
            position AS Position,
            created_at AS CreatedAt
        FROM BOARD_COLUMNS
        WHERE board_id = @BoardId
        ORDER BY position;";

        public const string SelectBoardMembers = @"
        SELECT
            u.id AS Id,
            u.user_name AS UserName,
            u.user_email AS UserEmail
        FROM USERS_PROFILE_DATA u
        INNER JOIN BOARD_USERS bu ON bu.user_id = u.id
        WHERE bu.board_id = @BoardId
        ORDER BY bu.joined_at;";

        public const string SelectUserByEmail = @"
        SELECT
            id AS Id,
            user_name AS UserName,
            user_email AS UserEmail
        FROM USERS_PROFILE_DATA
        WHERE user_email = @Email;";
        #endregion

        #region Updates
        public const string UpdateBoard = @"
        UPDATE BOARD
        SET
            name = COALESCE(@Name, name),
            description = COALESCE(@Description, description)
        WHERE id = @BoardId;";

        public const string UpdateTask = @"
        UPDATE BOARD_TASKS
        SET
            column_id = COALESCE(@ColumnId, column_id),
            task_owner_id = COALESCE(@TaskOwnerId, task_owner_id),
            task_type = COALESCE(@TaskType, task_type),
            task_name = COALESCE(@TaskName, task_name),
            task_description = COALESCE(@TaskDescription, task_description)
        WHERE id = @TaskId
        AND board_id = @BoardId;";
  
        public const string UpdateColumn = @"
        START TRANSACTION;

        UPDATE BOARD_COLUMNS
        SET position = position + 10000
        WHERE board_id = @BoardId
        AND id <> @ColumnId
        AND position BETWEEN LEAST(@OldPosition, @NewPosition) AND GREATEST(@OldPosition, @NewPosition);

        UPDATE BOARD_COLUMNS
        SET
            name = COALESCE(@Name, name),
            position = @NewPosition
        WHERE id = @ColumnId
        AND board_id = @BoardId;

        UPDATE BOARD_COLUMNS
        SET position = position - 10000 + IF(@NewPosition < @OldPosition, 1, -1)
        WHERE board_id = @BoardId
        AND position >= 10000;

        COMMIT;";
        #endregion

        #region Deletes
        public const string DeleteBoard = @"
        START TRANSACTION;

        DELETE FROM BOARD_TASKS
        WHERE board_id = @BoardId;

        DELETE FROM BOARD_COLUMNS
        WHERE board_id = @BoardId;

        DELETE FROM BOARD_USERS
        WHERE board_id = @BoardId;

        DELETE FROM BOARD
        WHERE id = @BoardId;

        COMMIT;";

        public const string DeleteTask = @"
        DELETE FROM BOARD_TASKS
        WHERE id = @TaskId
        AND board_id = @BoardId;";

        public const string DeleteColumn = @"
        START TRANSACTION;

        SET @deleted_position = (
            SELECT position
            FROM BOARD_COLUMNS
            WHERE id = @ColumnId
            AND board_id = @BoardId
        );

        DELETE FROM BOARD_TASKS
        WHERE column_id = @ColumnId
        AND board_id = @BoardId;

        DELETE FROM BOARD_COLUMNS
        WHERE id = @ColumnId
        AND board_id = @BoardId;

        UPDATE BOARD_COLUMNS
        SET position = position - 1
        WHERE board_id = @BoardId
        AND position > @deleted_position
        ORDER BY position;

        COMMIT;";

        public const string DeleteMember = @"
        START TRANSACTION;

        UPDATE BOARD_TASKS
        SET task_owner_id = NULL
        WHERE board_id = @BoardId
        AND task_owner_id = @UserId;

        DELETE FROM BOARD_USERS
        WHERE board_id = @BoardId
        AND user_id = @UserId;

        COMMIT;";
        #endregion
    }
}
