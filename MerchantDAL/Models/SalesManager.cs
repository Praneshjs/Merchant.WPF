using System.Collections.Generic;
using System.Linq;

namespace MerchantDAL.Models
{
    public class SalesManager
    {
        private Dictionary<int, List<SalesItemModel>> salesByBillId;
        private Dictionary<int, List<int>> purchaseItemId;

        private List<int> billId;

        private int currentBillId;

        public SalesManager()
        {
            salesByBillId = new Dictionary<int, List<SalesItemModel>>();
            purchaseItemId = new Dictionary<int, List<int>>();
            billId = new List<int>();
            currentBillId = 1;
        }

        public void AddSale(int billId, SalesItemModel sale)
        {
            if (!salesByBillId.ContainsKey(billId))
            {
                salesByBillId[billId] = new List<SalesItemModel>();
            }

            salesByBillId[billId].Add(sale);
        }

        public void AddItemId(int billId, int itemId)
        {
            if (!purchaseItemId.ContainsKey(billId))
            {
                purchaseItemId[billId] = new List<int>();
            }

            purchaseItemId[billId].Add(itemId);
        }

        public void AddBillId(int id)
        {
            if (!billId.Where(s => s == id).Any())
            {
                billId.Add(id);
            }
        }

        public void SetCurrentBillId(int id)
        {
            currentBillId = id;
        }

        public int GetCurrentBillId()
        {
            return currentBillId;
        }

        public int BillCount()
        {
            return billId.Count();
        }
        public void RemoveBillId(int id)
        {
            if (billId.Where(s => s == id).Any())
            {
                salesByBillId.Remove(id);
                billId.Remove(id);
            }
        }


        public void RemoveSale(int billId, SalesItemModel sale)
        {
            if (salesByBillId.ContainsKey(billId))
            {
                salesByBillId[billId].Remove(sale);
            }
        }
        public void RemoveItem(int billId, int itemId)
        {
            if (purchaseItemId.ContainsKey(billId))
            {
                purchaseItemId[billId].Remove(itemId);
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

        public List<int> GetAllItems(int billId)
        {
            if (purchaseItemId.ContainsKey(billId))
            {
                return purchaseItemId[billId];
            }
            else
            {
                return new List<int>();
            }
        }

        public int GetMaxBillId()
        {
            if (billId.Count() > 0)
            {
                return billId.Max();
            }
            else
            {
                return 1;
            }
        }

        public List<int> GetAllBillId()
        {
            return billId;
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
