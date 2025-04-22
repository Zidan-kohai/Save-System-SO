using Infrastructure.Data.PersistantData.Helper.Path;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Infrastructure.PersistantData.Remote.Helper.Path
{
    [CreateAssetMenu(fileName = "URL_PATH", menuName = "SO/Data/Helper/Path/URL Path")]
    public class URLPath : ScriptableObject, IDataPath
    {
        [field: SerializeField, ReadOnly] public string Path { get; protected set; }

        [Button]
        private void ChooseFile(string url)
        {
            Path = url;
        }
    }
}