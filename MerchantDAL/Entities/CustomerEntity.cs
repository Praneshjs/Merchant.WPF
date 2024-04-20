using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDAL
{
    public class CustomerEntity
    {
        public CustomerEntity()
        {
        }
        public async Task<List<Customer>> GetCustomerAsync(string allInfo)
        {
            using (var dbContext = new MerchantEntities())
            {
                allInfo = allInfo?.ToLower();
                var list = await dbContext.Customers
                    .Where(s => ((s.FirstName.ToLower() == allInfo || s.FirstName.ToLower().Contains(allInfo))
                    || (s.LastName.ToLower() == allInfo || s.LastName.ToLower().Contains(allInfo))
                    || (s.Mobile == allInfo || s.Mobile.Contains(allInfo))
                    || (s.AltMobile == allInfo || s.AltMobile.Contains(allInfo))
                    || (s.LandLine == allInfo || s.LandLine.Contains(allInfo))
                    || (s.EmailId.ToLower() == allInfo || s.EmailId.ToLower().Contains(allInfo))
                    || (s.AddressLineOne.ToLower() == allInfo || s.AddressLineOne.ToLower().Contains(allInfo))
                    || (s.AddressLineTwo.ToLower() == allInfo || s.AddressLineTwo.ToLower().Contains(allInfo))
                    || (s.City.ToLower() == allInfo || s.City.ToLower().Contains(allInfo))
                    || (s.PinCode.ToLower() == allInfo || s.PinCode.ToLower().Contains(allInfo)))
                    || string.IsNullOrEmpty(allInfo))
                    .ToListAsync();
                return list;
            }
        }

        public async Task<bool> IsCustomerDataExist(string mobile, string altMobile, string landLine, string email)
        {
            using (var context = new MerchantEntities())
            {
                return await context.Customers
                    .AnyAsync(s => ((s.Mobile == mobile || s.AltMobile == mobile) && !string.IsNullOrEmpty(mobile))
                    || ((s.Mobile == altMobile || s.AltMobile == altMobile) && !string.IsNullOrEmpty(altMobile))
                    || (s.LandLine == landLine && !string.IsNullOrEmpty(landLine))
                    || (s.EmailId == email && !string.IsNullOrEmpty(email)));
            }
        }

        public async Task<List<Customer>> SubmitCustomerAsync(CustomerModel customer)
        {
            using (var context = new MerchantEntities())
            {
                if (customer.Id > 0)
                {
                    var existingData = await context.Customers.FindAsync(customer.Id);
                    if (existingData != null)
                    {
                        existingData.AddressLineOne = customer.AddressLineOne;
                        existingData.AddressLineTwo = customer.AddressLineTwo;
                        existingData.AltMobile = customer.AltMobile;
                        existingData.EmailId = customer.EmailId;
                        existingData.FirstName = customer.FirstName;
                        existingData.LastName = customer.LastName;
                        existingData.Mobile = customer.Mobile;
                        existingData.City = customer.City;
                        existingData.ModifiedBy = customer.CreatedById;
                        existingData.ModifiedOn = DateTime.Now;
                        existingData.PinCode = customer.PinCode;
                        existingData.IsActive = customer.IsActive;
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var newData = new Customer
                    {
                        AddressLineOne = customer.AddressLineOne,
                        AddressLineTwo = customer.AddressLineTwo,
                        AltMobile = customer.AltMobile,
                        EmailId = customer.EmailId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Mobile = customer.Mobile,
                        City = customer.City,
                        CreatedBy = customer.CreatedById,
                        CreatedOn = DateTime.Now,
                        PinCode = customer.PinCode,
                        IsActive = customer.IsActive,
                    };

                    context.Customers.Add(newData);
                    await context.SaveChangesAsync();
                }
                var allData = await context.Customers.ToListAsync();
                return allData;
            }
        }
    }
}
