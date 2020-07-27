namespace LogBook.Repositories.MaxView
{
    public class DatabaseStatusRepositoryFactory
    {
        private static IDatabaseStatusRepository databaseStatusRepository;

        public static IDatabaseStatusRepository Create()
        {
            if (databaseStatusRepository != null)
                return databaseStatusRepository;
            return new DatabaseStatusRepository();
        }

    }
}
