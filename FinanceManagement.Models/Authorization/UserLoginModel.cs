using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Models.Authorization
{
    public class UserLoginModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
