using MerchantDAL;
using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantService.Services
{
    public class CustomerService
    {
        public async Task<List<CustomerModel>> GetCustomerAsync(string allInfo)
        {
            CustomerEntity custEntity = new CustomerEntity();
            var customers = await custEntity.GetCustomerAsync(allInfo);
            if (customers == null) return null;

            return customers.Select(s => new CustomerModel()
            {
                AddressLineOne = s.AddressLineOne,
                AddressLineTwo = s.AddressLineTwo,
                AltMobile = s.AltMobile,
                City = s.City,
                EmailId = s.EmailId,
                FirstName = s.FirstName,
                Id = s.Id,
                IsActive = s.IsActive??false,
                LandLine = s.LandLine,
                LastName = s.LastName,
                Mobile = s.Mobile,
                PinCode = s.PinCode
            }).ToList();
        }

        public async Task<List<CustomerModel>> SubmitCustomerAsync(CustomerModel customer)
        {
            CustomerEntity productMasterEntity = new CustomerEntity();
            var allData = await productMasterEntity.SubmitCustomerAsync(customer);
            return allData.Select(s => new CustomerModel()
             {
                 AddressLineOne = s.AddressLineOne,
                 AddressLineTwo = s.AddressLineTwo,
                 AltMobile = s.AltMobile,
                 City = s.City,
                 EmailId = s.EmailId,
                 FirstName = s.FirstName,
                 Id = s.Id,
                 IsActive = s.IsActive ?? false,
                 LandLine = s.LandLine,
                 LastName = s.LastName,
                 Mobile = s.Mobile,
                 PinCode = s.PinCode
             }).ToList();
        }

        public async Task<bool> IsCustomerDataExist(string mobile, string altMobile, string landLine, string email)
        {
            CustomerEntity customerEntity = new CustomerEntity();
            return await customerEntity.IsCustomerDataExist(mobile, altMobile, landLine, email);
        }
    }
}
