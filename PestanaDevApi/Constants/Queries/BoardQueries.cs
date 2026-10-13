namespace PestanaDevApi.Constants.Queries
{
    public static partial class BoardQueries
    {
        public const string Select = @"
        SELECT
            b.id AS Id,
            b.owner_id AS OwnerId,
            b.name AS Name,
            b.description AS Description,
            b.created_at AS CreatedAt,
            b.updated_at AS UpdatedAt
        FROM
            BOARD b
        INNER JOIN
            BOARD_USERS bu
        ON
            bu.board_id = b.id
        WHERE
            bu.user_id = @UserId
        ORDER BY
            b.created_at;";

        public const string SelectBoardData = @"
        SELECT
            id AS Id,
            owner_id AS OwnerId,
            name AS Name,
            description AS Description,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM
            BOARD
        WHERE
            id = @Id;

        SELECT
            id AS Id,
            board_id AS BoardId,
            name AS Name,
            position AS Position,
            created_at AS CreatedAt
        FROM
            BOARD_COLUMNS
        WHERE
            board_id = @Id;

        SELECT
            id AS Id,
            board_id AS BoardId,
            column_id AS ColumnId,
            task_owner_id AS TaskOwnerId,
            task_type AS TaskType,
            task_name AS TaskName,
            task_description AS TaskDescription,
            deadline AS Deadline,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM
            BOARD_TASKS
        WHERE
            board_id = @Id;

        SELECT
            u.id AS Id,
            u.user_name AS UserName,
            u.user_email AS UserEmail
        FROM
            USERS_PROFILE_DATA u
        INNER JOIN
            BOARD_USERS bu
        ON
            bu.user_id = u.id
        WHERE
            bu.board_id = @Id;";

        public const string SelectBoardById = @"
        SELECT
            id AS Id,
            owner_id AS OwnerId,
            name AS Name,
            description AS Description,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM
            BOARD
        WHERE
            id = @BoardId;";

        public const string SelectIsBoardMember = @"
        SELECT EXISTS(
            SELECT
                1
            FROM
                BOARD_USERS
            WHERE
                board_id = @BoardId
            AND
                user_id = @UserId
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
            deadline AS Deadline,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM
            BOARD_TASKS
        WHERE
            id = @TaskId
        AND
            board_id = @BoardId;";

        public const string SelectBoardColumns = @"
        SELECT
            id AS Id,
            board_id AS BoardId,
            name AS Name,
            position AS Position,
            created_at AS CreatedAt
        FROM
            BOARD_COLUMNS
        WHERE
            board_id = @BoardId
        ORDER BY
            position;";

        public const string SelectBoardMembers = @"
        SELECT
            u.id AS Id,
            u.user_name AS UserName,
            u.user_email AS UserEmail
        FROM
            USERS_PROFILE_DATA u
        INNER JOIN
            BOARD_USERS bu
        ON
            bu.user_id = u.id
        WHERE
            bu.board_id = @BoardId
        ORDER BY
            bu.joined_at;";

        public const string SelectUserByEmail = @"
        SELECT
            id AS Id,
            user_name AS UserName,
            user_email AS UserEmail
        FROM
            USERS_PROFILE_DATA
        WHERE
            user_email = @Email;";

        public const string SelectUserById = @"
        SELECT
            id AS Id,
            user_name AS UserName,
            user_email AS UserEmail
        FROM
            USERS_PROFILE_DATA
        WHERE
            id = @UserId;";

        public const string SelectIsUserInvited = @"
        SELECT EXISTS(
            SELECT
                1
            FROM
                BOARD_INVITATIONS
            WHERE
                board_id = @BoardId
            AND
                invited_email = @Email
        );";
    }
}
