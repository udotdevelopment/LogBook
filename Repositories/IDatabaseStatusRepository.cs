namespace LogBook.Repositories
{
    public interface IDatabaseStatusRepository
    {
        string GetStatusName(int StatusId);
    }
}