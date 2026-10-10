using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class AddBoardMemberRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
