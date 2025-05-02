using Infrastructure.Data.PersistantData.Save.Interface;
using System.Collections.Generic;

namespace Infrastructure.Data.PersistantData.Save.Saver
{
    public interface ISaver
    {
        public List<ISavable> savables { get; }

        public bool AddISavable(ISavable savable);
        
        public bool RemoveISavable(ISavable savable);
        
        public void Save();
    }
}