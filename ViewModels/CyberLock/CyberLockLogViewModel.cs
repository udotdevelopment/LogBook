﻿using System;
using System.Collections.Generic;

namespace LogBook.ViewModels.CyberLock
{
    public class CyberLockLogViewModel
    {
       
        public List<CyberLockLog> CyberlockLogs { get; set; }
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

        public CyberLockLogViewModel()
        {

        }

    }
}