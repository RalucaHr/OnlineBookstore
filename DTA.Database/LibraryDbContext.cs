using DTA.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace DTA.Database
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext() : base("name=BookstoreAppDB")
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public override int SaveChanges()
        {
            foreach (var history in this.ChangeTracker.Entries().Where(e => e.Entity is IModificationHistory &&
            (e.State == EntityState.Added || e.State == EntityState.Modified)).Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                    history.DateCreated = DateTime.Now;
            }

            int result = base.SaveChanges();

            foreach (var history in this.ChangeTracker.Entries().Where(e => e.Entity is IModificationHistory)
                .Select(e => e.Entity as IModificationHistory))
            {
                history.IsDirty = false;
            }
            return result;
        }
    }
}
