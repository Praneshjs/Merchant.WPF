using MerchantDAL.EntityModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDAL
{
    public class UserEntity
    {
        private readonly MerchantEntities _dbContext;
        public UserEntity()
        {
            _dbContext = new MerchantEntities();
        }

        public async Task<Profile> ValidateLoginAsync(string userName, string password)
        {
            return await _dbContext.Profiles
                .Where(s => s.UserName == userName && s.Password == password && (s.IsActive ?? false))
                .FirstAsync();
        }
    }
}
