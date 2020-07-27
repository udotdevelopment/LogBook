using System;

namespace LogBook.Repositories
{
    public class DatabaseStatusRepository : IDatabaseStatusRepository
    {
        private readonly Maxview db = new Maxview();

        public string GetStatusName(int StatusId)
        {
            var result = (from c in db.DatabaseStatus
                where c.Id == StatusId
                select c).FirstOrDefault();
            if (result != null)
            {
                return result.Name;
            }
            else
            {
                return String.Empty;
            }
        }

    }
}
