using MerchantDAL.EntityModel;
using System;
using System.ComponentModel;

namespace MerchantDAL.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.Guid> QRId { get; set; }
        public Nullable<decimal> StockPrice { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public decimal WeightKgs { get; set; }
        public virtual CommonData CommonData { get; set; }
        public virtual CommonData CommonData1 { get; set; }
        // Calculated property that concatenates Brand and ProductType
        public string FullProductName => $"{CommonData?.ControlValue} {CommonData1?.ControlValue}";
    }
}
