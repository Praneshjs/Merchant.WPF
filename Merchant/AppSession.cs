using MerchantDAL.Models;

namespace Merchant
{
    public class UserSession
    {
        private static UserSession instance;

        public SalesManager SalesManager { get; set; }

        public string Username { get; set; }
        public int UserId { get; set; }

        private UserSession()
        {
            Username = string.Empty;
            UserId = -1;
            SalesManager = new SalesManager();
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
