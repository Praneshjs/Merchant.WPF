//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MerchantDAL.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int Id { get; set; }
        public Nullable<System.Guid> QRId { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public int WeightTypeId { get; set; }
        public decimal ItemWeight { get; set; }
        public Nullable<decimal> StockPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsSold { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual CommonData CommonData { get; set; }
        public virtual CommonData CommonData1 { get; set; }
        public virtual CommonData CommonData2 { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Profile Profile1 { get; set; }
    }
}
