using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantDAL
{
    // Define error handling interface
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }

    public class ErrorHandler : IErrorHandler
    {
        public void HandleError(Exception ex)
        {
            // Handle error centrally (e.g., log, display message)
        }
    }
}
