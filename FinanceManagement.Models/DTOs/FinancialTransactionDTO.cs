namespace FinanceManagement.Models.DTOs
{
    public class FinancialTransactionDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
    }
}
