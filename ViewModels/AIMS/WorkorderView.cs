using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace LogBook.ViewModels.AIMS
{
    public class WorkorderView
    {
        public int ExtId { get; set; }
        public int IntId { get; set; }
        [DisplayFormat(DataFormatString = "MM/dd/yyyy HH:mm")]
        public DateTime DateCreated { get; set; }
        public string ShortDescription { get; set; }
        public string CreatedByUserName { get; set; }
        public string Status { get; set; }

        public string AimsUrl {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["AimsWorkOrderUrl"] + IntId;
            }
        }
    }
}