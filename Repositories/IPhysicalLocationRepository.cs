namespace LogBook.Repositories
{
    public interface IPhysicalLocationRepository
    {
        PhysicalLocation GetLocationForRWIS(string RWISLocation);
    }
}