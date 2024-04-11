using MerchantDAL.EntityModel;
using MerchantDAL.Models;
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
        public async Task<List<CommonControlModel>> GetCommonControlAsync()
        {
            using (var context = new MerchantEntities())
            {
                return await context.CommonControls.Where(s => s.IsActive).Select(t => new CommonControlModel()
                {
                    Id = t.Id,
                    CommonControlName = t.ControlType
                }).ToListAsync();
            }
        }

        public async Task<bool> IsProductMasterExist(int controlTypeId, string controlValue)
        {
            using (var context = new MerchantEntities())
            {
                return await context.CommonDatas
                    .AnyAsync(s => s.ControlTypeId == controlTypeId
                    && s.ControlValue == controlValue);
            }
        }

        public async Task<List<CommonData>> SubmitProductMasterAsync(string selectedId, int controlType, string controlValue, bool isActive)
        {
            using (var context = new MerchantEntities())
            {
                if (!string.IsNullOrEmpty(selectedId) && int.TryParse(selectedId, out int id))
                {
                    var existingData = await context.CommonDatas.FindAsync(id);
                    if (existingData != null)
                    {
                        existingData.ControlTypeId = controlType;
                        existingData.ControlValue = controlValue;
                        existingData.IsActive = isActive;

                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var newData = new CommonData
                    {
                        ControlTypeId = controlType,
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

        public async Task<List<CommonData>> GetProductMasterAsync(int controlTypeId, string controlValue, bool? isActive)
        {
            using (var context = new MerchantEntities())
            {
                var query = context.CommonDatas.AsQueryable();
                query = query.Where(s => (s.ControlTypeId == controlTypeId || controlTypeId == 0)
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
