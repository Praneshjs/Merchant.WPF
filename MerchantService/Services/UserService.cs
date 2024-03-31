using MerchantDAL;
using MerchantDAL.Models;
using System.Threading.Tasks;

namespace MerchantService
{
    public class UserService
    {
        public async Task<UserModel> ValidateLoginAsync(string username, string password)
        {
            UserEntity userEntity = new UserEntity();
            var profile = await userEntity.ValidateLoginAsync(username, password);
            if (profile == null) return null;

            return new UserModel()
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
