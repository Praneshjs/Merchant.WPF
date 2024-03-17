using MerchantDAL;
using MerchantDAL.EntityModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MerchantService.Services
{
    public class ProductMasterService
    {
        public async Task<List<CommonData>> SubmitProductMasterAsync(string selectedId, string controlType, string controlValue, bool isActive)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.SubmitProductMasterAsync(selectedId, controlType, controlValue, isActive);
        }

        public async Task<List<CommonData>> GetProductMasterAsync(string controlType, string controlValue, bool? isActive)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.GetProductMasterAsync(controlType, controlValue, isActive);
        }

        public async Task<bool> IsProductMasterExist(string controlType, string controlValue)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.IsProductMasterExist(controlType, controlValue);
        }
    }
}
