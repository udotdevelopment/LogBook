using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace LogBook.Models.LogBook
{
    public class Log
    {
        public int Id { get; set; }
        [Display(Name = "Date of Response")]
        [Index("IX_DateLocation", 1)]
        public DateTime Timestamp { get; set; }
        [NotMapped]
        public string SearchId { get; set; }
        [Display(Name = "Location ID")]
        [Index("IX_DateLocation", 2)]
        [MaxLength(50)]
        [Required]
        public string LocationId { get; set; }
        [NotMapped]
        [Display(Name = "Intersection")]
        public string Intersection { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        [Display(Name = "Location Type")]
        public LocationType LocationType { get; set; }
        [NotMapped]
        public string LocationTypeDescription { get; set; }
        [Required]
        [Display(Name = "Reason For Response")]
        public List<ReasonForResponse> ReasonForResponses { get; set; }
        [NotMapped]
        public string ReasonForResponseCommaSeparated
        {

            get
            {
                string commaList = String.Empty;
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

        [Required]
        [Display(Name = "Location of Response")]
        public bool Onsite { get; set; }
        [NotMapped]
        public bool Remote { get; set; }

        [NotMapped]
        [Display(Name = "Location of Response")]
        public string OnsiteOrRemote
        {
            get;
            set;
            //get
            //{
            //    if (Remote)
            //        return "Remote";
            //    else
            //        return "On Site";
            //}
        }
        [Required]
        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Required]
        public string User { get; set; }

        public sealed class LogClassMap : ClassMap<Log>
        {
            public LogClassMap()
            {
                //Map(m => m.Id).Name("ID");
                Map(m => m.Timestamp).Name("Date of Response");
                Map(m => m.LocationId).Name("Signal/Location Id");
                Map(m => m.Intersection).Name("Intersection");
                //Map(m => m.DateCreated).Name("Date Created");
                Map(m => m.OnsiteOrRemote).Name("Location of Response");
                Map(m => m.Comment).Name("Comment");
                Map(m => m.User).Name("User");
                Map(m => m.ReasonForResponseCommaSeparated).Name("Reason For Response");
            }
        }

    }
}