using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CF_Budgeter.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Amount { get; set; }
        public string TransactionTypeId { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }
        public decimal? ReconciledAmount { get; set; }

        public string UserId { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}