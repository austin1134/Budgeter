namespace CF_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HouseholdId = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        Name = c.String(),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ReconciledBalance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BudgetItems = c.String(),
                        HouseholdId = c.Int(nullable: false),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .ForeignKey("dbo.Households", t => t.HouseholdId, cascadeDelete: true)
                .Index(t => t.HouseholdId)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TransactionId = c.Int(nullable: false),
                        Households = c.Int(nullable: false),
                        Category_Id = c.Int(),
                        Budget_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Budgets", t => t.Budget_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Budget_Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Description = c.String(),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Amount = c.Double(nullable: false),
                        Type = c.String(),
                        CategoryId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Reconciled = c.Boolean(nullable: false),
                        ReconciledAmount = c.Double(nullable: false),
                        Transaction_Id = c.Int(),
                        Budget_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Budgets", t => t.Budget_Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.Transaction_Id)
                .Index(t => t.Budget_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Budget_Id = c.Int(),
                        Household_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.Budget_Id)
                .ForeignKey("dbo.Households", t => t.Household_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Budget_Id)
                .Index(t => t.Household_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Households",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Household_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Households", t => t.Household_Id)
                .Index(t => t.Household_Id);
            
            CreateTable(
                "dbo.BudgetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        Budgets = c.String(),
                        BudgetItem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id)
                .Index(t => t.BudgetItem_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.HouseholdAccounts",
                c => new
                    {
                        Household_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Household_Id, t.Account_Id })
                .ForeignKey("dbo.Households", t => t.Household_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Household_Id)
                .Index(t => t.Account_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.AspNetUsers", "Household_Id", "dbo.Households");
            DropForeignKey("dbo.Households", "Household_Id", "dbo.Households");
            DropForeignKey("dbo.Budgets", "HouseholdId", "dbo.Households");
            DropForeignKey("dbo.HouseholdAccounts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.HouseholdAccounts", "Household_Id", "dbo.Households");
            DropForeignKey("dbo.Budgets", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Budget_Id", "dbo.Budgets");
            DropForeignKey("dbo.AspNetUsers", "Budget_Id", "dbo.Budgets");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Categories", "Budget_Id", "dbo.Budgets");
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Transactions", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.Categories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.HouseholdAccounts", new[] { "Account_Id" });
            DropIndex("dbo.HouseholdAccounts", new[] { "Household_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.BudgetItems", new[] { "BudgetItem_Id" });
            DropIndex("dbo.Households", new[] { "Household_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Household_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Budget_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Transactions", new[] { "Budget_Id" });
            DropIndex("dbo.Transactions", new[] { "Transaction_Id" });
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            DropIndex("dbo.Transactions", new[] { "AccountId" });
            DropIndex("dbo.Categories", new[] { "Budget_Id" });
            DropIndex("dbo.Categories", new[] { "Category_Id" });
            DropIndex("dbo.Budgets", new[] { "Account_Id" });
            DropIndex("dbo.Budgets", new[] { "HouseholdId" });
            DropTable("dbo.HouseholdAccounts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.BudgetItems");
            DropTable("dbo.Households");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Transactions");
            DropTable("dbo.Categories");
            DropTable("dbo.Budgets");
            DropTable("dbo.Accounts");
        }
    }
}
