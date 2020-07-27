using System;

namespace LogBook.ViewModels.CyberLock
{
    public class CyberLockLog
    {
        public string Name { get; set; }  //lock audit report name. E.g.: <name>Audit Trail for lock 6142 (Serial #L601417D8)</name>
        public string Person_name { get; set; }
        public string Key_name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}