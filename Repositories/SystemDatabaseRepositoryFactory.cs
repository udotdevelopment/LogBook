namespace LogBook.Repositories
{
    public class SystemDatabaseRepositoryFactory
    {
        private static ISystemDatabaseRepository systemDatabaseRepository;

        public static ISystemDatabaseRepository Create()
        {
            if (systemDatabaseRepository != null)
                return systemDatabaseRepository;
            return new SystemDatabaseRepository();
        }

    }
}
