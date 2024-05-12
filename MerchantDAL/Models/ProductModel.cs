using MerchantDAL.EntityModel;
using System;
using System.Collections.Generic;

namespace MerchantDAL.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public int WeightTypeId { get; set; }
        public decimal ItemWeight { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.Guid QRId { get; set; } = Guid.NewGuid();
        public Nullable<decimal> StockPrice { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public virtual CommonData CommonData { get; set; }
        public virtual CommonData CommonData1 { get; set; }
        public virtual CommonData CommonData2 { get; set; }
        public string FullProductName => $"{CommonData?.ControlValue} {CommonData1?.ControlValue}";
        public string ProductWeight => $"{ItemWeight} {CommonData2?.ControlValue}";

    }

    public class ProductWithCount
    {
        public ProductModel Product { get; set; }
        public int AvailableQuantity { get; set; }

        public List<Guid> ProductQRId { get; set; } 
    }
}
