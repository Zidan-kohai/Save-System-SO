using System;
using System.Collections.Generic;
using System.ComponentModel;
using Infrastructure.Data.PersistantData.Save.Interface;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Data.PersistantData.Save.SaveComponent.SaveValueComponent
{
    public class SaveComponentBase<T> : SerializedScriptableObject, ISavable, ILoadable where T : struct
    {
        [field: SerializeField] public T Value { get; protected set; }
        [SerializeField] protected T defaultValue;
        
        [OnValueChanged("SetGlobalSaveStorage")]
        [SerializeField] private SaveConfig.SaveConfig saveConfig;
        [HideInInspector, SerializeField] private SaveConfig.SaveConfig lastSaveConfig;

        private List<Action<T>> actions = new List<Action<T>>();
        

        public void AddListener(Action<T> action)
        {
            actions.Add(action);
        }

        public void RemoveListener(Action<T> action)
        {
            if(actions.Contains(action))
                actions.Remove(action);
        }
        
        [Button]
        public void SetValue(T value)
        {
            Value = value;
            
            saveConfig?.Saver.Save();
            
            actions.ForEach(x => x.Invoke(value));
        }

        public void Reset() => SetValue(defaultValue);
        
        public virtual string GetName() => name;
        public virtual string GetValue() => Value.ToString();
        public virtual void Load(string value)
        { 
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                     
            if (converter.IsValid(value))
            {
                var loadValue = (T)converter.ConvertFromString(value)!;
                SetValue(loadValue);
            }
            else
            {
                Reset();
            }
                     
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
        
        [Button("Destroy")]
        protected virtual void Destroy()
        {
            RemoveFromGlobalSave();
#if UNITY_EDITOR
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
#endif
        }
    }
}