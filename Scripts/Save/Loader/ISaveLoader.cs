using System.Collections.Generic;
using Infrastructure.Data.PersistantData.Save.Interface;

namespace Infrastructure.Data.PersistantData.Save.Loader
{
    public interface ISaveLoader
    {
        public List<ILoadable> loadables { get; }
        
        public bool AddILoadable(ILoadable loadable);
        
        public bool RemoveILoadable(ILoadable loadable);
        
        public void Load();
    }
}