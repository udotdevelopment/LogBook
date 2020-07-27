using System;
using System.Collections.Generic;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;

namespace LogBook.Repositories.LogBook
{
    public interface ILogBookRepository
    {
        List<LogView> GetLogs(string signalId, string searchText);
        List<LogView> SearchLogs(string locationId, string commentSearch, DateTime startDate, DateTime endDate, string userSearch, int onsiteRemote, int reasons);
        void Update(Log log);
        int GetLogCount(string signalId, string searchText);
    }
}