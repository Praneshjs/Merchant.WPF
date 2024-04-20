using MerchantDAL.Entities;
using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MerchantService.Services
{
    public class ProductService
    {
        public async Task<List<ProductModel>> GetProductAsync(string searchText, bool? isActive)
        {
            ProductEntity productEntites= new ProductEntity();
            return await productEntites.GetProductAsync(searchText, isActive);
        }

        public async Task<List<ProductModel>> SubmitProductListAsync(List<ProductModel> products)
        {
            ProductEntity productEntites = new ProductEntity();
            return await productEntites.SubmitProductListAsync(products);
        }
    }
}
