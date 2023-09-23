using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Models.Authorization
{
    public class UserRegistrationModel : UserLoginModel
    {
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public DateTime? BirthDate { get; set; }

        public string? Nickname { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
