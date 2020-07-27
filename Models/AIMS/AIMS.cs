using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LogBook.Models.AIMS
{
    public class AIMS : DbContext
    {
        public AIMS() : base("name=AIMSConnection")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180; // 180 seconds?
        }
        public virtual DbSet<PhysicalLocation> PhysicalLocations { get; set; }
    }
}