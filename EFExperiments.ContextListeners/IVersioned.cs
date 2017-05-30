namespace EFExperiments.ContextListeners
{
    public interface IVersioned
    {
        byte[] RowVersion { get; set; }
    }
}