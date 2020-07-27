namespace LogBook.Repositories
{
    public class MaxviewUserRepositoryFactory
    {
        private static IMaxviewUserRepository maxviewUserRepository;

        public static IMaxviewUserRepository Create()
        {
            if (maxviewUserRepository != null)
                return maxviewUserRepository;
            return new MaxviewUserRepository();
        }

    }
}
