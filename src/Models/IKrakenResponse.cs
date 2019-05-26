namespace com.rossbrigoli.Yana
{
    public interface IKrakenResponse<T>
    {
        string[] Error { get; set; } 
        T Result { get; set; }
        
    }
}