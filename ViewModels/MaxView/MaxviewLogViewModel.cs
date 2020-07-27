using System;
using System.Collections.Generic;
using LogBook.Models.MaxView;

namespace LogBook.ViewModels.MaxView
{
    public class MaxViewLogViewModel
    {
        public List<MaxViewLog> MaxViewLogs { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalNumber { get; set; }

        public int TotalPages
        {
            get
            {
                double pagesVar = (Convert.ToDouble(TotalNumber) / Convert.ToDouble(PageSize));
                bool hasRemainder = (Convert.ToDouble(TotalNumber) % Convert.ToDouble(PageSize)) != 0;
                int pages = 0;
                if (pagesVar < 1)
                {
                    pages = 1;
                }
                else if (hasRemainder)
                {
                    pages = Convert.ToInt32(pagesVar + 1);
                }
                else
                {
                    pages = Convert.ToInt32(pagesVar);
                }

                return pages;
            }
        }

        public MaxViewLogViewModel()
        {
        }

    }

}
