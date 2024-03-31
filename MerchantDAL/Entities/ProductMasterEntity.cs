using MerchantDAL.EntityModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDAL
{
    public class ProductMasterEntity
    {
        public ProductMasterEntity()
        {
        }

        public async Task<bool> IsProductMasterExist(string controlType, string controlValue)
        {
            using (var context = new MerchantEntities())
            {
                return await context.CommonDatas
                    .AnyAsync(s => s.ControlType == controlType 
                    && s.ControlValue == controlValue);
            }
        }

        public async Task<List<CommonData>> SubmitProductMasterAsync(string selectedId, string controlType, string controlValue, bool isActive)
        {
            using (var context = new MerchantEntities())
            {
                if (!string.IsNullOrEmpty(selectedId) && int.TryParse(selectedId, out int id))
                {
                    var existingData = await context.CommonDatas.FindAsync(id);
                    if (existingData != null)
                    {
                        existingData.ControlType = controlType;
                        existingData.ControlValue = controlValue;
                        existingData.IsActive = isActive;

                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var newData = new CommonData
                    {
                        ControlType = controlType,
                        ControlValue = controlValue,
                        IsActive = isActive
                    };

                    context.CommonDatas.Add(newData);
                    await context.SaveChangesAsync();
                }
                var allData = await context.CommonDatas.ToListAsync();
                return allData;
            }
        }

        public async Task<List<CommonData>> GetProductMasterAsync(string controlType, string controlValue, bool? isActive)
        {
            using (var context = new MerchantEntities())
            {
                var query = context.CommonDatas.AsQueryable();
                query = query.Where(s => s.ControlType.ToLower() == controlType.ToLower() || s.ControlType.ToLower().Contains(controlType.ToLower()) || string.IsNullOrEmpty(controlType)
                || s.ControlValue.ToLower() == controlValue.ToLower() || s.ControlValue.ToLower().Contains(controlValue.ToLower()) || string.IsNullOrEmpty(controlValue));

                // Apply filters conditionally based on provided parameters
                //if (!string.IsNullOrWhiteSpace(controlType))
                //{
                //    query = query.Where(s => s.ControlType.ToLower() == controlType.ToLower() || s.ControlType.ToLower().Contains(controlType.ToLower()));
                //}
                //if (!string.IsNullOrWhiteSpace(controlValue))
                //{
                //    query = query.Where(s => s.ControlValue.ToLower() == controlValue.ToLower() || s.ControlValue.ToLower().Contains(controlValue.ToLower()));
                //}
                if (isActive != null)
                {
                    query = query.Where(s => s.IsActive == isActive);
                }

                var allData = await query.ToListAsync();
                return allData;
            }
        }
    }
}
