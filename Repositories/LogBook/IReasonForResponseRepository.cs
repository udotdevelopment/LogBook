using System.Collections.Generic;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;

namespace LogBook.Repositories.LogBook
{
    public interface IReasonForResponseRepository
    {
        List<ReasonForResponseAPIView> GetReasons();
    }
}