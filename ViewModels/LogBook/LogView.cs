using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;
using LogBook.ViewModels.AIMS;

namespace LogBook.ViewModels.LogBook
{
    public class LogView
    {
        public int Id { get; set; }
        [Display(Name = "Date of Response")]
        [Index("IX_DateLocation", 1)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Timestamp { get; set; }

        public string SearchId { get; set; }

        [Display(Name = "Location ID")]
        [Index("IX_DateLocation", 2)]
        [MaxLength(50)]
        public string LocationId { get; set; }
        [Display(Name = "Intersection")]
        public string Intersection { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Location Type")]
        public string LocationTypeDescription { get; set; }

        [Display(Name = "Location Description")]
        public string LocationDescription { get; set; }

        [Display(Name = "Reason For Response")]
        public List<ReasonForResponseView> ReasonForResponses { get; set; }

        public string ReasonForResponseCommaSeparated
        {
            
            get { string commaList = String.Empty;
                for (int i = 0; i < ReasonForResponses.Count; i++)
                {
                    commaList += ReasonForResponses[i].Abbreviation;
                    if (i < ReasonForResponses.Count - 1)
                    {
                        commaList += ", ";
                    }
                }

                return commaList;
            }
                
        }

        //[Display(Name = "Work Order Number")]
        //public int? WorkOrderNumber { get; set; }

        [Display(Name = "Location of Response")]
        public bool Onsite { get; set; }
        public bool Remote { get; set; }
        [Display(Name = "Location of Response")]
        public string OnsiteOrRemote { get; set; }


        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public string User { get; set; }



        public sealed class LogClassMap : ClassMap<LogView>
        {
            public LogClassMap()
            {
                Map(m => m.Id).Name("ID");
                Map(m => m.Timestamp).Name("Timestamp");
                Map(m => m.LocationId).Name("Signal Id");
                Map(m => m.Intersection).Name("Intersection");
                Map(m => m.DateCreated).Name("Date Created");
                Map(m => m.OnsiteOrRemote).Name("Location of Response");
                Map(m => m.Comment).Name("Comment");
                Map(m => m.User).Name("User");
            }
        }
    }
}