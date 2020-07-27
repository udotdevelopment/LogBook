using System;

namespace LogBook.Models.MaxView
{
    public class MaxViewLog
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
    }
}