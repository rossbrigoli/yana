namespace com.rossbrigoli.Yana
{
    public interface IKrakenResponse<T>
    {
        string[] Error { get; set; } 
        T Result { get; set; }
        
    }

    //TODO: Remove this and refactor other converters to use generic KrakenJsonConverter
    public interface IKrakenResponse
    {
        string[] Error { get; set; }
        
    }
}