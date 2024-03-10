using MerchantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant
{
    public class AppSession
    {
        private static readonly Lazy<UserProfile> _currentUser = new Lazy<UserProfile>(() => new UserProfile());

        public static UserProfile CurrentUser => _currentUser.Value;
    }
}
