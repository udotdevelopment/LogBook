namespace LogBook.Repositories.MaxView
{
    public class SystemDatabaseTypeRepositoryFactory
    {
        private static ISystemDatabaseTypeRepository systemDatabaseTypeRepository;

        public static ISystemDatabaseTypeRepository Create()
        {
            if (systemDatabaseTypeRepository != null)
                return systemDatabaseTypeRepository;
            return new SystemDatabaseTypeRepository();
        }

    }
}
