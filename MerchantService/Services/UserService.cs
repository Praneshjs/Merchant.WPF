using MerchantDAL;
using MerchantService.Models;
using System.Threading.Tasks;

namespace MerchantService
{
    public class UserService
    {
        public async Task<UserProfile> ValidateLoginAsync(string username, string password)
        {
            UserEntity userEntity = new UserEntity();
            var profile = await userEntity.ValidateLoginAsync(username, password);
            if (profile == null) return null;

            return new UserProfile()
            {
                EmailId = profile.EmailId,
                FirstName = profile.FirstName,
                Id = profile.Id,
                Mobile = profile.Mobile,
                UserName = profile.UserName
            };
        }
    }
}
