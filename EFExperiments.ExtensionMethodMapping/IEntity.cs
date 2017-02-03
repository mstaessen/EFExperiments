namespace EFExperiments.ExtensionMethodMapping
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}