namespace LogBook.Repositories.MaxView
{
    public class GroupableElementRepositoryFactory
    {
        private static IGroupableElementRepository groupableElementRepository;

        public static IGroupableElementRepository Create()
        {
            if (groupableElementRepository != null)
                return groupableElementRepository;
            return new GroupableElementRepository();
        }

    }
}
