using System.Linq;
using LogBook.Models.AIMS;

namespace LogBook.Repositories.AIMS
{
    public class PhysicalLocationRepository : IPhysicalLocationRepository
    {
        private readonly Models.AIMS.AIMS db = new Models.AIMS.AIMS();

        public PhysicalLocation GetLocationForRWIS(string RWISLocation)
        {
            string commaed = "RWIS-" + RWISLocation.ToUpper();
            string spaced = commaed + " ";
            commaed += ",";
            var result = db.PhysicalLocations
                .Where(m => (m.Description.ToUpper().Contains(commaed)
                            || m.Description.ToUpper().Contains(spaced)))
                .FirstOrDefault();

            return result;
        }
    }
}
