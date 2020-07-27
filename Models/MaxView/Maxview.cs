using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LogBook.Models.MaxView
{
    public class Maxview : DbContext
    {
        public Maxview() : base("name=MaxviewConnection")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180; // 180 seconds?
        }
        public virtual DbSet<MaxviewUser> MaxviewUsers { get; set; }
        public virtual DbSet<DatabaseChange> DatabaseChanges { get; set; }
        public virtual DbSet<DatabaseStatus> DatabaseStatus { get; set; }
        public virtual DbSet<GroupableElement> GroupableElements { get; set; }
        public virtual DbSet<SystemDatabase> SystemDatabases { get; set; }
        public virtual DbSet<SystemDatabaseType> SystemDatabaseTypes { get; set; }

    }
}