namespace LogBook.Repositories.MaxView
{
    public interface IGroupableElementRepository
    {
        int GetMaxviewIdFromSignalId(int SignalId);
    }
}