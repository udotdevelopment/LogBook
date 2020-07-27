using System.Collections.Generic;
using LogBook.Models.MaxView;

namespace LogBook.Repositories.MaxView
{
    public interface ISystemDatabaseRepository
    {
        List<SystemDatabase> GetDownloadListFromSignalId(int SignalId);
    }
}