using System.Collections.Generic;
using System.Linq;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;

namespace LogBook.Repositories.LogBook
{
    public class ReasonForResponseRepository : IReasonForResponseRepository
    {
        private readonly Models.LogBook.LogBook db = new Models.LogBook.LogBook();

        public List<ReasonForResponseAPIView> GetReasons()
        {
            return db.ReasonForResponses.Select(r => new ReasonForResponseAPIView
                {
                    Id = r.Id, Abbreviation = r.Abbreviation, Description = r.Description, DisplayOrder = r.DisplayOrder
                })
                .ToList();
        }


    }
}
