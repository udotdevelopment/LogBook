using LogBook.Models.AIMS;

namespace LogBook.Repositories.AIMS
{
    public interface IPhysicalLocationRepository
    {
        PhysicalLocation GetLocationForRWIS(string RWISLocation);
    }
}