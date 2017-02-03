namespace EFExperiments.ExtensionMethodMapping
{
    public interface IVersionedEntity
    {
        byte[] RowVersion { get; set; }
    }
}