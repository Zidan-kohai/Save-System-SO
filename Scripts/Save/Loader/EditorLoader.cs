using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Data.PersistantData.Helper.Path;
using Infrastructure.Data.PersistantData.Save.Interface;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Data.PersistantData.Save.Loader
{
    [CreateAssetMenu(fileName = "EDITOR_LOADER", menuName = "SO/Data/Persistant Data/Save/Editor/Loader")]
    public sealed class EditorLoader : SerializedScriptableObject, ISaveLoader
    {
        [SerializeField] private IDataPath dataPath;
        [field: OdinSerialize] public List<ILoadable> loadables { get; set; } = new();
        
        public bool AddILoadable(ILoadable loadable)
        {
            if (loadables.Contains(loadable))
            {
                Debug.LogWarning($"The Editor Loader already contains a savable of type {loadable.GetType()}");
                return false;
            }
            
            loadables.Add(loadable);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            #endif
            return true;
        }

        public bool RemoveILoadable(ILoadable loadable)
        {
            if (!loadables.Contains(loadable))
            {
                Debug.LogWarning($"The Editor Loader doesnt contains a savable of type {loadable.GetType()}");
                return false;
            }
            
            loadables.Remove(loadable);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            #endif
            return true;
        }

        public void Load()
        {
            if (dataPath == null || string.IsNullOrEmpty(dataPath.Path) || string.IsNullOrWhiteSpace(dataPath.Path))
                throw new Exception("Something go wrong, Data Path field is null or dataPath.filePath is empty, null, or only white space");


            string json = File.ReadAllText(dataPath.Path);

            Dictionary<string, string> saveData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();

            foreach (var loadable in loadables)
            {
                string name = loadable.GetName();
                
                if (saveData.TryGetValue(name, out var value))
                {
                    loadable.Load(value); 
                }
                else
                    loadable.Load(null);
            }
        }
    }
}