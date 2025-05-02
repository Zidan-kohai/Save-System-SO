using System.Collections.Generic;

namespace Infrastructure.Data.PersistantData.Save.SaveComponent.SaveCollectionComponent.Dictionary
{
    public class DictionaryCollectionSaveComponent<T, V> : CollectionSaveComponenntBase<Dictionary<T, V>, KeyValuePair<T, V>> where T : struct where V : struct
    {

        public void AddValue(T key, V Value)
        {
            AddValue(new KeyValuePair<T, V>(key, Value));
        }

        public override void AddValue(KeyValuePair<T, V> keyValue)
        {
            if (collectionValue is null)
                Reset();

            collectionValue.Add(keyValue.Key, keyValue.Value);

            saveConfig.Saver.Save();

            actions.ForEach(x => x.Invoke(keyValue));
        }
    }
}