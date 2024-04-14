using MerchantDAL.EntityModel;

namespace MerchantDAL.Models
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>();
            // Add more CreateMap calls for other mappings if needed
        }
    }
}
