namespace Infrastructure.Data.PersistantData.Save.Interface
{
    public interface ILoadable
    {
        public string GetName();
        public void Load(string value);
    }
}