using MerchantDAL;
using MerchantDAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantService.Services
{
    public class ProductMasterService
    {
        public async Task<List<CommonData>> SubmitProductMasterAsync(string selectedId, string controlType, string controlValue, bool isActive)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            var productMaster = await productMasterEntity.SubmitProductMasterAsync(selectedId, controlType, controlValue, isActive);
            return productMaster;
        }

        public async Task<List<CommonData>> GetProductMasterAsync(string controlType, string controlValue, bool? isActive)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            var productMaster = await productMasterEntity.GetProductMasterAsync(controlType, controlValue, isActive);
            return productMaster;
        }

        
    }
}
