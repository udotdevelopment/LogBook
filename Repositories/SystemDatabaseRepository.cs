using System.Collections.Generic;

namespace LogBook.Repositories
{
    public class SystemDatabaseRepository : ISystemDatabaseRepository
    {
        private readonly Maxview db = new Maxview();
        private IGroupableElementRepository grpElementRepository = GroupableElementRepositoryFactory.Create();

        public List<SystemDatabase> GetDownloadListFromSignalId(int SignalId)
        {
            int MaxviewId = grpElementRepository.GetMaxviewIdFromSignalId(SignalId);
            var SysDbList = db.SystemDatabases
                .Where(m => m.DeviceId == MaxviewId
                       && (m.SystemDatabaseTypeID == 4 || m.SystemDatabaseTypeID == 5))
                .OrderByDescending(m => m.StartTime).ToList();
            return SysDbList;
        }

    }
}
