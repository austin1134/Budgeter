using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace CF_Budgeter.Models
{
    public class Household
    {
        public Household()
        {
        this.Accounts = new HashSet<Account>();
        this.Budgets = new HashSet<Budget>();
        this.Members = new HashSet<ApplicationUser>();
        this.Categories = new HashSet<Category>();
        this.Invitations = new HashSet<Invitations>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account>Accounts { get; set; }
        public virtual ICollection<Budget>Budgets { get; set; }
        public virtual ICollection<ApplicationUser>Members { get; set; }
        public virtual ICollection<Category>Categories { get; set; }
        public virtual ICollection<Invitations>Invitations { get; set; }  
    }
}