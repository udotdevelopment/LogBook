using System.Collections.Generic;

namespace LogBook.Repositories
{
    public interface ISystemDatabaseRepository
    {
        List<SystemDatabase> GetDownloadListFromSignalId(int SignalId);
    }
}