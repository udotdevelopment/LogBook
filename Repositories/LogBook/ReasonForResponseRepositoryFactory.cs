namespace LogBook.Repositories.LogBook
{
    public class ReasonForResponseRepositoryFactory
    {
        private static IReasonForResponseRepository ReasonForResponseRepository;

        public static IReasonForResponseRepository Create()
        {
            if (ReasonForResponseRepository != null)
                return ReasonForResponseRepository;
            return new ReasonForResponseRepository();
        }

    }
}
