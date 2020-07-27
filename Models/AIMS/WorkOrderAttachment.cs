using System;

namespace LogBook.Models.AIMS
{
    public class WorkOrderAttachment
    {
        public int IntId { get; set; }

        public int ExtId { get; set; }

        public int WorkOrderIntId { get; set; }

        public AttachmentType AttachmentType { get; set; }

        public string Filename { get; set; }

        public byte[] Attachment { get; set; }

        public DateTime DateCreated { get; set; }

        public User CreatedByUser { get; set; }

        public string SessionKey { get; set; }
    }
}