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
        public async Task<List<CommonData>> SubmitProductMasterAsync(string selectedId, string controlType, string controlValue, bool isActive)
        {
            using (var context = new MerchantEntities())
            {
                if (!string.IsNullOrEmpty(selectedId) && int.TryParse(selectedId, out int id))
                {
                    // Retrieve the existing CommonData entity by its primary key (id)
                    var existingData = await context.CommonDatas.FindAsync(id);
                    if (existingData != null)
                    {
                        // Update the properties of the existing entity
                        existingData.ControlType = controlType;
                        existingData.ControlValue = controlValue;
                        existingData.IsActive = isActive;

                        // Save changes to the database
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    // Create a new CommonData entity if selectedId is not provided or invalid
                    var newData = new CommonData
                    {
                        ControlType = controlType,
                        ControlValue = controlValue,
                        IsActive = isActive
                    };

                    context.CommonDatas.Add(newData);
                    await context.SaveChangesAsync(); // Use asynchronous SaveChanges method
                }

                // Retrieve all CommonData entities from the database
                var allData = await context.CommonDatas.ToListAsync();
                return allData;
            }
        }

        public async Task<List<CommonData>> GetProductMasterAsync(string controlType, string controlValue, bool? isActive)
        {
            using (var context = new MerchantEntities())
            {
                var query = context.CommonDatas.AsQueryable();

                // Apply filters conditionally based on provided parameters
                if (controlType != null)
                {
                    query = query.Where(s => s.ControlType == controlType);
                }
                if (controlValue != null)
                {
                    query = query.Where(s => s.ControlValue == controlValue);
                }
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
