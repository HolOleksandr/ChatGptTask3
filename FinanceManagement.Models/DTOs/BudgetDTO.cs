using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Models.DTOs
{
    public class BudgetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
        public ICollection<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
    }
}
