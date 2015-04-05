using System.Data.Entity;
using System.Threading.Tasks;
using Zombie.Models;

namespace Zombie.Services
{
    public sealed class ClaimService : DbService, IClaimService
    {
        public ClaimService(SiteContext context) : base(context) { }

        public async Task<Claim> Get(string type, string value)
        {
            return await Context.Claims.SingleAsync(c => c.Type == type && c.Value == value);
        }
    }

    public interface IClaimService : IDbService
    {
        Task<Claim> Get(string type, string value);
    }
}
