using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GalaxyVibesPos.Models
{
    public class DatabaseContext:DbContext
    {
        public DbSet<CategoryMain> CategoryMain { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<CategorySub> CategorySub { get; set; }

        public DbSet<ProductDetails> productDetails { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<TarminalInformation> TerminalInformation { get; set; }
        public DbSet<Customer> Customer { get; set; }      
        public DbSet<CustomerLedger> CustomerLedger { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SupplierGroup> SupplierGroup { get; set; }
        public DbSet<SupplierCompany> SupplierCompany { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<SupplierLedger> SupplierLedger { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
    }
}