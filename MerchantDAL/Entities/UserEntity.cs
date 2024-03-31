using MerchantDAL.EntityModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDAL
{
    public class UserEntity
    {
        public UserEntity()
        {
        }
        public async Task<Profile> ValidateLoginAsync(string userName, string password)
        {
            using (var dbContext = new MerchantEntities())
            {
                return await dbContext.Profiles
                    .Where(s => s.UserName == userName && s.Password == password && (s.IsActive ?? false))
                    .FirstAsync();
            }
        }
    }
}
