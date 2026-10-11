using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class AddBoardMemberRequestDto
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Locale of the invitation email sent when the user is not registered yet.
        /// </summary>
        [Required]
        public string ClientLocale { get; set; } = string.Empty;
    }
}
