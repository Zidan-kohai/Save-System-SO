using Infrastructure.Data.PersistantData.Save.Interface;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Data.PersistantData.Save.SaveComponent.SaveCollectionComponent
{
    public class CollectionSaveComponenntBase<T, V> : SerializedScriptableObject, ILoadable, ISavable where T : ICollection<V> where V : struct
    {
        [field: OdinSerialize] public T collectionValue { get; protected set; } = Activator.CreateInstance<T>();
        [SerializeField] protected T defaultValue = Activator.CreateInstance<T>();

        [OnValueChanged("SetGlobalSaveStorage")]
        [SerializeField] protected SaveConfig.SaveConfig saveConfig;
        [HideInInspector, SerializeField] protected SaveConfig.SaveConfig lastSaveConfig;

        protected List<Action<V>> actions = new List<Action<V>>();


        public string GetName() => name;

        public string GetValue()
        {
            string json = JsonConvert.SerializeObject(collectionValue);

            return json;
        }

        public void Load(string value)
        {
            try
            {
                collectionValue = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
                Reset();
            }
        }

        public void Reset()
        {
            collectionValue?.Clear();
            collectionValue = defaultValue;
        }

        public void AddListener(Action<V> action)
        {
            actions.Add(action);
        }

        public void RemoveListener(Action<V> action)
        {
            if (actions.Contains(action))
                actions.Remove(action);
        }

        public virtual void AddValue(V value)
        {
            if(collectionValue is null)
                Reset();

            collectionValue.Add(value);

            saveConfig.Saver.Save();

            actions.ForEach(x => x.Invoke(value));
        }

        [Button("Save", ButtonSizes.Medium), PropertySpace(SpaceBefore = 10)]
        public void Save()
        {
            saveConfig.Saver.Save();
        }

        private void SetGlobalSaveStorage()
        {
            RemoveFromGlobalSave();
            AddToGlobalSave();
        }
        private void AddToGlobalSave()
        {
            if (saveConfig == null) return;

            saveConfig.SaveLaoder.AddILoadable(this);
            saveConfig.Saver.AddISavable(this);

            lastSaveConfig = saveConfig;
        }
        private void RemoveFromGlobalSave()
        {
            if (lastSaveConfig != null)
            {
                lastSaveConfig.SaveLaoder.RemoveILoadable(this);
                lastSaveConfig.Saver.RemoveISavable(this);
            }
        }

        [Button("Destroy", ButtonSizes.Medium), PropertySpace(SpaceBefore = 10)]
        [HorizontalGroup("Center")]
        protected virtual void Destroy()
        {
            RemoveFromGlobalSave();
#if UNITY_EDITOR
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
#endif
        }
    }
}