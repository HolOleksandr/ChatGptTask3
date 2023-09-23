namespace FinanceManagement.Models.DTOs
{
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
    }
}
