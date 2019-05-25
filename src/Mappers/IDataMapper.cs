using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    /// <summary>
    /// All JSON data mappers must implement this interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataMapper<T>
    {
        T MapFields(JObject jo);
    }
}