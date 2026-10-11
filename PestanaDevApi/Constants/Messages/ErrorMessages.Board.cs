namespace PestanaDevApi.Constants.Messages
{
    public partial class ErrorMessages
    {
        #region Board
        public const string BoardNotFound = $"6A3C : Board was not found!";
        public const string BoardNotUpdated = $"2F7B : Board was not updated due internal errors!";
        public const string BoardNotDeleted = $"9B4E : Board was not deleted due internal errors!";
        public const string BoardRequestDontHaveAnyChangedData = $"C3D2 : Cannot update board because no changes were detected.";
        public const string UserIsNotBoardMember = $"7E1A : You are not a member of this board!";
        public const string UserIsNotBoardOwner = $"E8C5 : Only the board owner can do this action!";
        public const string BoardColumnsNamesMustBeUnique = $"A7D3 : The board columns names must be different from each other!";
        #endregion

        #region Board Tasks
        public const string TaskNotFound = $"3B9F : Task was not found!";
        public const string TaskNotUpdated = $"5D2C : Task was not updated due internal errors!";
        public const string TaskNotDeleted = $"A4E7 : Task was not deleted due internal errors!";
        public const string TaskRequestDontHaveAnyChangedData = $"1F6D : Cannot update task because no changes were detected.";
        public const string TaskOwnerIsNotBoardMember = $"B2A8 : The task owner must be a member of this board!";
        #endregion

        #region Board Columns
        public const string ColumnNotFound = $"8C1B : Column was not found!";
        public const string ColumnNotUpdated = $"D6B3 : Column was not updated due internal errors!";
        public const string ColumnRequestDontHaveAnyChangedData = $"E2F4 : Cannot update column because no changes were detected.";
        public const string ColumnNameAlreadyExists = $"6D8A : This board already has a column with this name!";
        #endregion

        #region Board Members
        public const string MemberNotFound = $"C9E1 : Member was not found in this board!";
        public const string MemberNotAdded = $"7B6F : Member was not added due internal errors!";
        public const string MemberNotRemoved = $"3E4B : Member was not removed due internal errors!";
        public const string UserIsAlreadyBoardMember = $"F5C7 : This user is already a member of this board!";
        public const string BoardOwnerCannotBeRemoved = $"2C8D : The board owner cannot be removed from the board!";
        public const string UserWasNotInvitedToBoard = $"5E9B : You were not invited to this board!";
        public const string MemberNotInvited = $"C2F8 : The invitation was not created due internal errors!";
        public const string ClientSideSystemUrlNotConfigured = $"7A4E : The client side system url is not configured!";
        #endregion
    }
}
