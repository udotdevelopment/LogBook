using System.Collections.Generic;
using LogBook.ViewModels.API;

namespace LogBook.Repositories
{
    public interface IDatabaseChangeRepository
    {
        List<DatabaseChange> GetDbChangesBySignal(int SignalId);
    }
}