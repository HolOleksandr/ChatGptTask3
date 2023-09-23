using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core
{
    public class PlaidTransaction
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string EncryptedName { get; set; } = null!;
        public string EncryptedCategory { get; set; } = null!;
        public string EncryptedAmount { get; set; } = null!;
        public DateTime Date { get; set; }
        public string EncryptedPlaidAccessToken { get; set; } = null!;
    }
}
