using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantDAL.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string AltMobile { get; set; }
        public string LandLine { get; set; }
        public string EmailId { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
