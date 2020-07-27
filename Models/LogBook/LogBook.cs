using System.Data.Entity;

namespace LogBook.Models.LogBook
{
    public class LogBook : DbContext
    {
        // Your context has been configured to use a 'LogBook' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ElectronicLogBookWebAPI.Models.LogBook.LogBook' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'LogBook' 
        // connection string in the application configuration file.
        public LogBook()
            : base("name=LogBook")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<ReasonForResponse> ReasonForResponses { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}