using System;

namespace LogBook.Models.AIMS
{
    public class Staff
    {
        public int IntId { get; set; }

        public int ExtId { get; set; }

        public string FirstName { get; set; }

        public string MiddleNameOrInitial { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string FirstLastName { get; set; }

        public string LastFirstName { get; set; }

        public string Title { get; set; }

        public string WebUserName { get; set; }

        public string WebEncryptedPassword { get; set; }

        public bool IsTerminated { get; set; }

        public DateTime? TerminationDate { get; set; }

        public bool IsAccountLockOut { get; set; }

        public bool? IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastUpdated { get; set; }

        public byte[] UserPicture { get; set; }

        public string ProfilePictureBase64 { get; set; }
    }
}