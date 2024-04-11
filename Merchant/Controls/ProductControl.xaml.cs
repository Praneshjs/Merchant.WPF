using MerchantDAL.Models;
using MerchantService.QR;
using MerchantService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;

namespace Merchant.Controls
{
    /// <summary>
    /// Interaction logic for ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        private int currentPageIndex = 1;
        private int itemsPerPage = 20;
        public ProductControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            GetAllCustomerAsync(currentPageIndex);
            LoadControlTypeAsync();
        }
        private async void LoadControlTypeAsync()
        {
            ProductMasterService fetch = new ProductMasterService();
            var masterDatas = await fetch.GetProductMasterAsync(0, null, null);
            var brandList = masterDatas.Where(s => s.CommonControl.ControlType == "Brand")
                .Select(t => new CommonDataModel() { Id = t.Id, ControlValue = t.ControlValue }).ToList();
            brandList.Add(new CommonDataModel { Id = 0, ControlValue = "Select Brand" });
            cmbBrandName.ItemsSource = brandList.OrderBy(s => s.Id);
            cmbBrandName.SelectedIndex = 0;

            var productCategoryList = masterDatas.Where(s => s.CommonControl.ControlType == "Product Category")
                .Select(t => new CommonDataModel() { Id = t.Id, ControlValue = t.ControlValue }).ToList();
            productCategoryList.Add(new CommonDataModel { Id = 0, ControlValue = "Select Product Type" });
            cmbProductType.ItemsSource = productCategoryList.OrderBy(s => s.Id);
            cmbProductType.SelectedIndex = 0;
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

        private void btnProductList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(cmbBrandName.SelectedValue?.ToString(), out int brandNameId);
                int.TryParse(cmbProductType.SelectedValue?.ToString(), out int productTypeId);
                var weight = txtWeight.Text;
                var quantity = txtQuantity.Text;
                var stockPrice = txtStockPrice.Text;
                var sellingPrice = txtSellingPrice.Text;

                StringBuilder validationMsg = new StringBuilder();
                if (string.IsNullOrEmpty(weight)) validationMsg.AppendLine("Product weight is empty");
                if (string.IsNullOrEmpty(quantity)) validationMsg.AppendLine("Product quantity is empty");
                if (string.IsNullOrEmpty(stockPrice)) validationMsg.AppendLine("Stock price is empty");
                if (string.IsNullOrEmpty(sellingPrice)) validationMsg.AppendLine("Selling price is empty");
                if (brandNameId == 0) validationMsg.AppendLine("Select a brand name");
                if (productTypeId == 0) validationMsg.AppendLine("Select a product type");
                
                if (validationMsg.Length > 0)
                {
                    validationMsgCtrl.ShowValidationBox(validationMsg.ToString());
                    return;
                }
                
                //QRService productQR = new QRService();
                //var qrImage = productQR.GenerateQRCode(new Guid().ToString());

                //var newData = new CustomerModel
                //{
                //    Id = customerId,
                //    AddressLineOne = addressLineOne,
                //    AddressLineTwo = addressLineTwo,
                //    AltMobile = altMobile,
                //    EmailId = email,
                //    FirstName = firstName,
                //    LastName = lastName,
                //    Mobile = mobile,
                //    City = city,
                //    CreatedById = 1,//UserSession.Instance.UserId,
                //    PinCode = pinCode,
                //    IsActive = true,
                //};
                //CustomerService fetchService = new CustomerService();
                //var allData = await fetchService.SubmitCustomerAsync(newData);
                //BindCustomerGridData(allData, currentPageIndex);
                //validationMsgCtrl.ShowValidationBox("New customer added successfully");
                btnClearCustomer_Click(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClearCustomer_Click(object sender, RoutedEventArgs e)
        {
            cmbBrandName.SelectedIndex = 0;
            cmbProductType.SelectedIndex = 0;
            txtWeight.Text = string.Empty;
            //btnAddCustomer.Tag = string.Empty;
            //txtFirstName.Text = string.Empty;
            //txtLastName.Text = string.Empty;
            //txtMobile.Text = string.Empty;
            //txtAltMobile.Text = string.Empty;
            //txtLandLine.Text = string.Empty;
            //txtEmail.Text = string.Empty;
            //txtAddressLineOne.Text = string.Empty;
            //txtAddressLineTwo.Text = string.Empty;
            //txtCity.Text = string.Empty;
            //txtPinCode.Text = string.Empty;
            //validationMsgCtrl.CloseValidationBox_Click(null, null);
            LoadControlTypeAsync();
        }

        private void btnSelectCustomer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (sender is Image image && image.Tag is CustomerModel selectedData)
            //{
            //    txtAddressLineOne.Text = selectedData.AddressLineOne;
            //    txtAddressLineTwo.Text = selectedData.AddressLineTwo;
            //    txtAltMobile.Text = selectedData.AltMobile;
            //    txtEmail.Text = selectedData.EmailId;
            //    txtFirstName.Text = selectedData.FirstName;
            //    txtLastName.Text = selectedData.LastName;
            //    txtMobile.Text = selectedData.Mobile;
            //    txtCity.Text = selectedData.City;
            //    txtPinCode.Text = selectedData.PinCode;
            //    btnAddCustomer.Tag = selectedData.Id;
            //}
        }

        private void txtWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (txtWeight.Text.IndexOf('.') > 0 && e.Text == ".")
            {
                e.Handled = true;
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]*\\.?[0-9]*$"))
            {
                e.Handled = true;
            }
        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true;
            }

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
