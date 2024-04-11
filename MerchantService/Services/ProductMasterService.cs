using MerchantDAL;
using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MerchantService.Services
{
    public class ProductMasterService
    {
        public async Task<List<CommonData>> SubmitProductMasterAsync(string selectedId, int controlTypeId, string controlValue, bool isActive)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.SubmitProductMasterAsync(selectedId, controlTypeId, controlValue, isActive);
        }

        public async Task<List<CommonData>> GetProductMasterAsync(int controlTypeId, string controlValue, bool? isActive)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.GetProductMasterAsync(controlTypeId, controlValue, isActive);
        }

        public async Task<bool> IsProductMasterExist(int controlTypeId, string controlValue)
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.IsProductMasterExist(controlTypeId, controlValue);
        }

        public async Task<List<CommonControlModel>> GetCommonControlAsync()
        {
            ProductMasterEntity productMasterEntity = new ProductMasterEntity();
            return await productMasterEntity.GetCommonControlAsync();
        }
    }
}
