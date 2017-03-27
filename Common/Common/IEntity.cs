namespace Common
{
    public interface IEntity<TID>
    {
        TID ID { get; set; }
    }
}