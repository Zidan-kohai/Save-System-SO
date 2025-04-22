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

namespace Infrastructure.Data.PersistantData.Save.Saver
{
    [CreateAssetMenu(fileName = "EDITOR_SAVER", menuName = "SO/Data/Persistant Data/Save/Editor/Saver")]
    public sealed class EditorSaver : SerializedScriptableObject, ISaver
    {
        [SerializeField] private IDataPath dataPath;
        [field: OdinSerialize] public List<ISavable> savables { get; set; } = new();

        public bool AddISavable(ISavable savable)
        {
            if (savables.Contains(savable))
            {
                Debug.LogWarning($"The Editor Saver already contains a savable of type {savable.GetType()}");
                return false;
            }
            
            savables.Add(savable);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            #endif
            return true;
        }

        public bool RemoveISavable(ISavable savable)
        {
            if (!savables.Contains(savable))
            {
                Debug.LogWarning($"The Editor Saver doesnt contains a savable of type {savable.GetType()}");
                return false;
            }
            
            savables.Remove(savable);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            #endif
            return true;
        }

        [Button]
        public void Save()
        {
            if (dataPath == null || string.IsNullOrEmpty(dataPath.Path) || string.IsNullOrWhiteSpace(dataPath.Path))
                throw new Exception("Something go wrong, Data Path field is null or dataPath.filePath is empty, null, or only white space");

            Dictionary<string, string> saveData = new Dictionary<string, string>();
        
            foreach (var savable in savables)
            {
                string name = savable.GetName();
                string value = savable.GetValue();
                if (!string.IsNullOrEmpty(name))
                {
                    saveData[name] = value;
                }
            }

            string json = JsonConvert.SerializeObject(saveData);

            File.WriteAllText(dataPath.Path, json);
        }
    }
}