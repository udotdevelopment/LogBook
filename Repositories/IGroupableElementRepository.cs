namespace LogBook.Repositories
{
    public interface IGroupableElementRepository
    {
        int GetMaxviewIdFromSignalId(int SignalId);
    }
}