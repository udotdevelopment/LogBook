using System;

namespace LogBook.Repositories
{
    public class SystemDatabaseTypeRepository : ISystemDatabaseTypeRepository
    {
        private readonly Maxview db = new Maxview();

        public string GetNameFromID(int Id)

        {
            var result = (from c in db.SystemDatabaseTypes
                where c.Id == Id
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
