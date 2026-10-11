namespace PestanaDevApi.Constants.Messages
{
    public class SuccessMessages
    {
        public const string DefaultSuccessMessage = "Ok";

        #region User
        public const string UserDataUpdated = "User data updated!";
        public const string UserDeleted = "User data was successfully deleted!";
        public const string UserPasswordChangeded = "User password was successfully changed!";
        #endregion

        #region Board
        public const string BoardUpdated = "Board data updated!";
        public const string BoardDeleted = "Board was successfully deleted!";
        public const string TaskUpdated = "Task was successfully updated!";
        public const string TaskDeleted = "Task was successfully deleted!";
        public const string ColumnUpdated = "Column was successfully updated!";
        public const string MemberAdded = "Member was successfully added!";
        public const string MemberInvited = "The user is not registered yet, so an invitation to sign up was sent by email!";
        public const string MemberRemoved = "Member was successfully removed!";
        #endregion
    }
}
