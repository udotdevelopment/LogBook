using System.Collections.Generic;
using System.Linq;
using LogBook.Models.MaxView;

namespace LogBook.Repositories.MaxView
{
    public class DatabaseChangeRepository : IDatabaseChangeRepository
    {
        private readonly Maxview db = new Maxview();

        public List<DatabaseChange> GetDbChangesBySignal(int SignalId)
        {
            var results = (from c in db.DatabaseChanges
                where c.SystemDatabaseID == SignalId
                select c).ToList();
            return results;
        }

    }
}
