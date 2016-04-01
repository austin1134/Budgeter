using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CF_Budgeter.Models
{
    public class InviteUsersViewModel
    {
        public IEnumerable<ApplicationUser>Members { get; set; }
        public IEnumerable<string>SelectedMembers { get; set; }
    }
}