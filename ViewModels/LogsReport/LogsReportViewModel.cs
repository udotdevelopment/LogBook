using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;

namespace LogBook.ViewModels.LogsReport
{
    public class LogsReportViewModel
    {
        public LogBookView LogsView { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Signal/Location")]
        public string LocationId { get; set; }
        [Display(Name = "Comment")]
        public string CommentSearch { get; set; }
        [Display(Name = "User")]
        public string UserSearch { get; set; }
        public bool OnSite { get; set; }
        public bool Remote { get; set; }
        public List<ReasonForResponseView> ReasonForResponses { get; set; }

        public int ReportPageNumber { get; set; }
        public LogsReportViewModel()
        {
            DateTime nowDate = DateTime.Now;
            EndDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0);
            StartDate = EndDate.AddMonths(-1);
            OnSite = true;
            Remote = true;
            CommentSearch = "";
            LogsView = new LogBookView();
            LogsView.Logs = new List<LogView>();
            LogsView.PageSize = 20;
            LogsView.PageNumber = 1;
            ReasonForResponses = new List<ReasonForResponseView>();
        }

    }

}
