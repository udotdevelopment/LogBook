using System.Linq;
using LogBook.Models.MaxView;

namespace LogBook.Repositories.MaxView
{
    public class GroupableElementRepository : IGroupableElementRepository
    {
        private readonly Maxview db = new Maxview();

        public int GetMaxviewIdFromSignalId(int SignalId)
        {
            var result = (from c in db.GroupableElements
                where c.Number == SignalId
                select c).FirstOrDefault();
            if (result != null)
            {
                return result.Id;
            }
            else
            {
                return 0;
            }
        }

    }
}
