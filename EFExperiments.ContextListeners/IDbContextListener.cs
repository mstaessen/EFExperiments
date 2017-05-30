using System.Data.Entity;

namespace EFExperiments.ContextListeners
{
    public interface IDbContextListener
    {
        void OnBeforeSaveChanges(DbContext context);
    }
}