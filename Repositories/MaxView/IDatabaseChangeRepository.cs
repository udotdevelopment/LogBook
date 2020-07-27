using System.Collections.Generic;
using LogBook.Models.MaxView;

namespace LogBook.Repositories.MaxView
{
    public interface IDatabaseChangeRepository
    {
        List<DatabaseChange> GetDbChangesBySignal(int SignalId);
    }
}