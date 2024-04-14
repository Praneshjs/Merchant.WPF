using AutoMapper;
using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDAL.Entities
{
    public class ProductEntity
    {
        private readonly IMapper _mapper;
        public ProductEntity()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.CreateMap<ProductModel, Product>();
            });
            _mapper = config.CreateMapper();
        }


        public async Task<List<ProductModel>> GetProductAsync(int brandId, int productTypeId, DateTime? expiryDate, string searchText, bool? isActive)
        {
            using (var context = new MerchantEntities())
            {
                var query = context.Products.AsQueryable();
                query = query.Where(s =>
                (s.BrandId == brandId || brandId == 0)
                || (s.ProductTypeId == productTypeId || productTypeId == 0)
                || (s.ExpiryDate == expiryDate || expiryDate == null)
                || s.CommonData.ControlValue.ToLower() == searchText.ToLower() || s.CommonData.ControlValue.ToLower().Contains(searchText.ToLower()) || string.IsNullOrEmpty(searchText));

                if (isActive != null)
                {
                    query = query.Where(s => s.IsActive == isActive);
                }

                var allData = await query.Include(t => t.CommonData).ToListAsync();
                return _mapper.Map<List<ProductModel>>(allData);
            }
        }

        public async Task<List<ProductModel>> SubmitProductListAsync(List<ProductModel> products)
        {
            using (var context = new MerchantEntities())
            {
                var updateDataExist = products.Any(p => p.Id > 0);

                if (updateDataExist)
                {
                    foreach (var product in products)
                    {
                        if (product.Id > 0)
                        {
                            var existingProduct = await context.Products.FindAsync(product.Id);
                            if (existingProduct != null)
                            {
                                _mapper.Map(product, existingProduct);
                            }
                        }
                    }
                }
                var newDatas = _mapper.Map<List<Product>>(products.Where(p => p.Id == 0));
                context.Products.AddRange(newDatas);

                await context.SaveChangesAsync();
                var allData = await context.Products.Include(s => s.CommonData).Include(s => s.CommonData1).ToListAsync();
                return _mapper.Map<List<ProductModel>>(allData);
            }
        }
    }
}
