using System.Collections.Generic;
using System.Linq;

namespace MerchantDAL.Models
{
    public class SalesManager
    {
        private Dictionary<int, List<SalesItemModel>> salesByBillId;

        public SalesManager()
        {
            salesByBillId = new Dictionary<int, List<SalesItemModel>>();
        }

        public void AddSale(int billId, SalesItemModel sale)
        {
            if (!salesByBillId.ContainsKey(billId))
            {
                salesByBillId[billId] = new List<SalesItemModel>();
            }

            salesByBillId[billId].Add(sale);
        }

        public void RemoveSale(int billId, SalesItemModel sale)
        {
            if (salesByBillId.ContainsKey(billId))
            {
                salesByBillId[billId].Remove(sale);
            }
        }

        public List<SalesItemModel> GetAllSales(int billId)
        {
            if (salesByBillId.ContainsKey(billId))
            {
                return salesByBillId[billId];
            }
            else
            {
                return new List<SalesItemModel>();
            }
        }

        public string GetTotalItemsPrice(int billId)
        {
            if (salesByBillId.ContainsKey(billId))
            {
                decimal totalPrice = salesByBillId[billId].Sum(item => item.ItemSetPrice);
                return totalPrice.ToString("F2"); // Format to 2 decimal places
            }
            else
            {
                return "0.00"; // Return 0 if no sales for the bill id
            }
        }
    }
}
