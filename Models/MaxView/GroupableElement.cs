using System;

namespace LogBook.Models.MaxView
{
    public class GroupableElement
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public Int16 ParentId { get; set; }
        public string Note { get; set; }
    }
}