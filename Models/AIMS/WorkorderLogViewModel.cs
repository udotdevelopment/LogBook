using System.Collections.Generic;
using LogBook.ViewModels.AIMS;

namespace LogBook.Models.AIMS
{
    public class WorkorderLogViewModel
    {
        public List<WorkorderView> WorkorderLogs { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalNumber { get; set; }

        public WorkorderLogViewModel()
        {
        }

    }

}
