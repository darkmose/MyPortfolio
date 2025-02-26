using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class BaseSaveData
    {
        public bool IsLocked;
        public List<BaseObjectSaveData> ObjectsData;
        public BaseSaveData()
        {
            ObjectsData = new List<BaseObjectSaveData>();
        }
    }
}