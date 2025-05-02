using Infrastructure.Data.PersistantData.Save.Loader;
using Sirenix.OdinInspector;
using UnityEngine;

public class Boot : SerializedMonoBehaviour
{
    [SerializeField] private ISaveLoader _loader;


    private void Awake()
    {
        _loader.Load();
    }
}
