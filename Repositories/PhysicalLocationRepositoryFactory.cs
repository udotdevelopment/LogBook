namespace LogBook.Repositories
{
    public class PhysicalLocationRepositoryFactory
    {
        private static IPhysicalLocationRepository physicalLocationRepository;

        public static IPhysicalLocationRepository Create()
        {
            if (physicalLocationRepository != null)
                return physicalLocationRepository;
            return new PhysicalLocationRepository();
        }

    }
}
