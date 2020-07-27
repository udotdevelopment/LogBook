namespace LogBook.Repositories.MaxView
{
    public class DatabaseChangeRepositoryFactory
    {
        private static IDatabaseChangeRepository databaseChangeRepository;

        public static IDatabaseChangeRepository Create()
        {
            if (databaseChangeRepository != null)
                return databaseChangeRepository;
            return new DatabaseChangeRepository();
        }

    }
}
