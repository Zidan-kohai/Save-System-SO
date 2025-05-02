using UnityEngine;

namespace Infrastructure.Data.PersistantData.Save.SaveComponent.SaveValueComponent.Float
{
    [CreateAssetMenu(fileName = "FLOAT_SAVE", menuName = "SO/Data/Persistant Data/Save/Component/Float")]
    public class FloatSaveComponent : SaveComponentBase<float>
    {
        public override void Load(string value)
        {
            if (float.TryParse(value, out float result))
            {
                SetValue(result);
            }
            else
            {
                base.Load(value);
            }
        }
    }
}