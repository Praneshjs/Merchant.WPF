using System;
using System.Collections.Generic;

namespace MerchantDAL.Models
{
    public class SalesItemModel
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public string FullProductName { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public decimal Weight { get; set; }
        public decimal Quantity { get; set; }
        public decimal ItemSetPrice => Math.Round(Quantity * (SellingPrice ?? 0), 2);

        // Override ToString method to return ItemSetPrice with 2 decimal places
        public override string ToString()
        {
            return ItemSetPrice.ToString("F2");
        }
    }


}
