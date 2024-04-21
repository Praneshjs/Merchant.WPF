using System.Collections.Generic;
using System.Linq;

namespace MerchantDAL.Models
{
    public class PurchaseItemsModel
    {
        public int BillId { get; set; }
        public List<SalesItemModel> SalesItems { get; set; }

        public string TotalItemsPrice => (SalesItems?.Sum(item => item.ItemSetPrice) ?? 0).ToString("F2");
    }

}
