using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CF_Budgeter.Models
{
    public class CreateTransactionViewModel
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

        public Transaction Transaction { get;  set; }

        public SelectList Categories { get; set; }
    }
}