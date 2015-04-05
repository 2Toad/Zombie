namespace Zombie.Services
{
    public abstract class DbService : IDbService
    {
        public SiteContext Context { get; private set; }

        protected DbService(SiteContext context)
        {
            Context = context;
        }
    }

    public interface IDbService
    {
        SiteContext Context { get; }
    }
}
