namespace CSharpTools.HTTP.Architecture
{
    public interface IJsonSerializer
    {
        public T? DeserializeObject<T>(string jsonString);
    }
}