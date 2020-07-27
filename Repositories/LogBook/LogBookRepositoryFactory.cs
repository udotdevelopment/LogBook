namespace LogBook.Repositories.LogBook
{
    public class LogBookRepositoryFactory
    {
        private static ILogBookRepository LogBookRepository;

        public static ILogBookRepository Create()
        {
            if (LogBookRepository != null)
                return LogBookRepository;
            return new LogBookRepository();
        }

    }
}
