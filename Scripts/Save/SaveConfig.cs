using Infrastructure.Data.PersistantData.Save.Loader;
using Infrastructure.Data.PersistantData.Save.Saver;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Infrastructure.Data.PersistantData.Save.SaveConfig
{
    [CreateAssetMenu(fileName = "SAVE_CONFIG", menuName = "SO/Data/Persistant Data/Save/SaveConfig")]
    public class SaveConfig : SerializedScriptableObject  
    {
        [field: OdinSerialize] public ISaveLoader SaveLaoder { get; } 
        [field: OdinSerialize] public ISaver Saver { get; }
    }
}