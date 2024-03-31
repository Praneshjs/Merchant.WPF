using MerchantDAL.Models;
using MerchantService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Merchant.Controls
{
    /// <summary>
    /// Interaction logic for CustomerControl.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        private int currentPageIndex = 1;
        private int itemsPerPage = 20;
        public CustomerControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            GetAllCustomerAsync(currentPageIndex);
        }

        private async void GetAllCustomerAsync(int pageIndex, string allInfo = null, bool? isActive = null)
        {
            CustomerService fetch = new CustomerService();
            var allData = await fetch.GetCustomerAsync(allInfo);
            BindCustomerGridData(allData, pageIndex);
        }
        private void BindCustomerGridData(List<CustomerModel> allData, int pageIndex)
        {
            int startIndex = (pageIndex * itemsPerPage) - itemsPerPage + 1;
            int endIndex = (startIndex - 1) + itemsPerPage;
            int skipCount = startIndex - 1;
            var totalPages = (int)Math.Ceiling((decimal)allData.Count() / itemsPerPage);
            UpdatePaginationInfo(pageIndex, totalPages);
            var paginationList = allData.Skip(skipCount).Take(itemsPerPage);

            lstViewCustomer.ItemsSource = null;
            lstViewCustomer.ItemsSource = paginationList;
        }

        private async void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstName = txtFirstName.Text;
                var lastName = txtLastName.Text;
                var mobile = txtMobile.Text;
                var altMobile = txtAltMobile.Text;
                var landLine = txtLandLine.Text;
                var email = txtEmail.Text;
                var addressLineOne = txtAddressLineOne.Text;
                var addressLineTwo = txtAddressLineTwo.Text;
                var city = txtCity.Text;
                var pinCode = txtPinCode.Text;

                StringBuilder validationMsg = new StringBuilder();
                if (string.IsNullOrEmpty(firstName)) validationMsg.AppendLine("First name is empty");
                if (string.IsNullOrEmpty(mobile)) validationMsg.AppendLine("Mobile is empty");
                if (validationMsg.Length == 0)
                {
                    CustomerService fetch = new CustomerService();
                    var isDataExist = await fetch.IsCustomerDataExist(mobile, altMobile, landLine, email);
                    if (isDataExist) validationMsg.AppendLine($"Customer info: Mobile {mobile} \n Alt. Mobile {altMobile},\n Landline {landLine},\n Email {email} already exist.");
                }
                if (validationMsg.Length > 0)
                {
                    validationMsgCtrl.ShowValidationBox(validationMsg.ToString());
                    return;
                }

                var newData = new CustomerModel
                {
                    AddressLineOne = addressLineOne,
                    AddressLineTwo = addressLineTwo,
                    AltMobile = altMobile,
                    EmailId = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Mobile = mobile,
                    City = city,
                    CreatedById = 1,//UserSession.Instance.UserId,
                    PinCode = pinCode,
                    IsActive = true,
                };
                CustomerService fetchService = new CustomerService();
                var allData = await fetchService.SubmitCustomerAsync(newData);
                BindCustomerGridData(allData, currentPageIndex);
                validationMsgCtrl.ShowValidationBox("New customer added successfully");
                btnClearCustomer_Click(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClearCustomer_Click(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtAltMobile.Text = string.Empty;
            txtLandLine.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddressLineOne.Text = string.Empty;
            txtAddressLineTwo.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            validationMsgCtrl.CloseValidationBox_Click(null, null);
        }

        private void btnSelectCustomer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void UpdatePaginationInfo(int currentPage, int totalPages)
        {
            customerPagination.SetPageInfo(currentPage, totalPages);
        }

        private void customerPagination_FirstPageClicked(object sender, EventArgs e)
        {

        }

        private void customerPagination_PreviousPageClicked(object sender, EventArgs e)
        {

        }

        private void customerPagination_NextPageClicked(object sender, EventArgs e)
        {

        }

        private void customerPagination_LastPageClicked(object sender, EventArgs e)
        {

        }
    }
}
