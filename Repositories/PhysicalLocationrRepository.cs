namespace LogBook.Repositories
{
    public class PhysicalLocationRepository : IPhysicalLocationRepository
    {
        private readonly AIMS db = new AIMS();

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
