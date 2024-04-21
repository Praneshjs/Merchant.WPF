using AutoMapper;
using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
                cfg.CreateMap<Product, ProductModel>();
                cfg.CreateMap<ProductModel, Product>();
                //.ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.CommonData.ControlValue))
                //.ForMember(dest => dest.ProductCategoryName, opt => opt.MapFrom(src => src.CommonData1.ControlValue));

                cfg.CreateMap<CommonDataModel, CommonData>();
                cfg.CreateMap<UserModel, MerchantDAL.EntityModel.Profile>();
            });
            _mapper = config.CreateMapper();
        }

        public async Task<List<ProductModel>> GetProductAsync(string searchText, bool? isActive)
        {
            using (var context = new MerchantEntities())
            {
                string[] monthpattern = { "MM-yy", "MMM-yy", "MM-yyyy", "MMM-yyyy", "MM/yy", "MMM/yy", "MM/yyyy", "MMM/yyyy" };
                var monthFilterStatus = DateTime.TryParseExact(searchText, monthpattern, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime monthParam);
                string[] datePattern = { "dd-MM-yy", "d-M-yy", "dd/MM/yy", "d/M/yy" };
                var dateFilterStatus = DateTime.TryParseExact(searchText, datePattern, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dateParam);
                var activeFilterStatus = bool.TryParse(searchText, out bool IsActiveFilter);

                var query = context.Products.AsQueryable();

                if (monthFilterStatus)
                {
                    query = query.Where(s => (s.ExpiryDate.Value.Month == monthParam.Month && s.ExpiryDate.Value.Year == monthParam.Year)
                    || (s.MfgDate.Value.Month == monthParam.Month && s.MfgDate.Value.Year == monthParam.Year));
                }
                else if (dateFilterStatus)
                {
                    query = query.Where(s => (s.ExpiryDate == dateParam) || (s.MfgDate == dateParam));
                }
                else if (activeFilterStatus)
                {
                    query = query.Where(s => s.IsActive == IsActiveFilter);
                }
                else
                {
                    query = query.Where(s => (s.CommonData.ControlValue.ToLower() == searchText.ToLower()
                    || s.CommonData.ControlValue.ToLower().Contains(searchText.ToLower()))
                    || (s.CommonData1.ControlValue.ToLower() == searchText.ToLower()
                    || s.CommonData1.ControlValue.ToLower().Contains(searchText.ToLower()))
                    || (s.WeightKgs.ToString() == searchText || s.WeightKgs.ToString().Contains(searchText))
                    || (s.StockPrice.ToString() == searchText || s.StockPrice.ToString().Contains(searchText))
                    || (s.SellingPrice.ToString() == searchText || s.SellingPrice.ToString().Contains(searchText))
                    || string.IsNullOrEmpty(searchText)
                    );
                }
                

                var allData = await query.Include(t => t.CommonData).Include(t => t.CommonData1)
                    .OrderByDescending(t => t.CreatedOn).ToListAsync();
                var result = _mapper.Map<List<ProductModel>>(allData);
                return result;
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
                var allData = await context.Products.Include(s => s.CommonData).Include(s => s.CommonData1)
                    .OrderByDescending(t => t.CreatedOn).ToListAsync();
                return _mapper.Map<List<ProductModel>>(allData);
            }
        }
    }
}
