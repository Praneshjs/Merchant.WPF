using MerchantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant
{
    public class UserSession
    {
        private static UserSession instance;

        public string Username { get; set; }
        public int UserId { get; set; }

        private UserSession()
        {
            Username = string.Empty;
            UserId = -1;
        }

        public static UserSession Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserSession();
                }
                return instance;
            }
        }
    }
}
