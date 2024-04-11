using Merchant.Helper;
using MerchantDAL.EntityModel;
using MerchantDAL.Models;
using MerchantService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Merchant.Controls
{
    public partial class ProductMasterControl : UserControl
    {
        private int currentPageIndex = 1;
        private int itemsPerPage = 20;
        public ProductMasterControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            GetAllProductMasterAsync(currentPageIndex);
            LoadControlTypeComboBoxAsync();
        }


        private async void GetAllProductMasterAsync(int pageIndex, int controlTypeId = 0, string controlValue = null, bool? isActive = null)
        {
            ProductMasterService fetch = new ProductMasterService();
            var allData = await fetch.GetProductMasterAsync(controlTypeId, controlValue, isActive);
            BindProductMasterGridData(allData, pageIndex);
        }

        private async void LoadControlTypeComboBoxAsync()
        {
            var commonControls = new List<CommonControlModel>();
            commonControls.Add(new CommonControlModel { Id = 0, CommonControlName = "Select" });
            ProductMasterService fetch = new ProductMasterService();
            var otherData = await fetch.GetCommonControlAsync();
            foreach (var item in otherData)
            {
                commonControls.Add(item);
            }
            cmbControlType.ItemsSource = commonControls;
            cmbControlType.SelectedIndex = 0;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validationMsgCtrl.CloseValidationBox_Click(null, null);
                ProductMasterService fetch = new ProductMasterService();
                int.TryParse(cmbControlType.SelectedValue?.ToString(), out int controlTypeId);
                var controlValue = txtControlValue.Text.Trim().ToTitleCase();
                var isActive = chkIsActive.IsChecked;

                StringBuilder validationMsg = new StringBuilder();
                if (controlTypeId == 0) validationMsg.AppendLine("Control name is empty");
                if (string.IsNullOrEmpty(controlValue)) validationMsg.AppendLine("Control value is empty");
                var isDataExist = await fetch.IsProductMasterExist(controlTypeId, controlValue);
                if (isDataExist) validationMsg.AppendLine($"Master Data: {cmbControlType.SelectedValue} and {controlValue} already exist.");
                if (validationMsg.Length > 0)
                {
                    validationMsgCtrl.ShowValidationBox(validationMsg.ToString());
                    return;
                }

                var selectedId = txtSelectedId.Text;
                var allData = await fetch.SubmitProductMasterAsync(selectedId, controlTypeId, controlValue, (bool)isActive);
                BindProductMasterGridData(allData, currentPageIndex);
                validationMsgCtrl.ShowValidationBox("Product master created successfully");
                ClearButton_Click(null, null);
            }
            catch (System.Exception)
            {
                validationMsgCtrl.ShowValidationBox("A technical error has occurred, Kindly contact app admin.");
            }
        }

        private void BindProductMasterGridData(List<CommonData> allData, int pageIndex)
        {
            int startIndex = (pageIndex * itemsPerPage) - itemsPerPage + 1;
            int endIndex = (startIndex - 1) + itemsPerPage;
            int skipCount = startIndex - 1;
            var totalPages = (int)Math.Ceiling((decimal)allData.Count() / itemsPerPage);
            UpdatePaginationInfo(pageIndex, totalPages);
            var paginationList = allData.Skip(skipCount).Take(itemsPerPage);

            productDataGrid.ItemsSource = null;
            productDataGrid.ItemsSource = paginationList;
        }


        private void txtControlType_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]+$"))
            {
                e.Handled = true;
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Image image && image.Tag is CommonData selectedData)
            {
                cmbControlType.SelectedItem = selectedData.ControlTypeId;
                txtControlValue.Text = selectedData.ControlValue;
                chkIsActive.IsChecked = selectedData.IsActive;
                txtSelectedId.Text = selectedData.Id.ToString();
            }
        }
        private void Row_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is CommonData rowData)
            {
                cmbControlType.SelectedItem = rowData.ControlTypeId;
                txtControlValue.Text = rowData.ControlValue;
                chkIsActive.IsChecked = rowData.IsActive;
                txtSelectedId.Text = rowData.Id.ToString();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            cmbControlType.SelectedIndex = 0;
            txtControlValue.Text = string.Empty;
            chkIsActive.IsChecked = true;
            txtSelectedId.Text = string.Empty;
            txtProductSearch.Text = string.Empty;
            validationMsgCtrl.CloseValidationBox_Click(null, null);
            if (sender != null)
                GetAllProductMasterAsync(currentPageIndex);
        }

        // Method to update pagination information
        private void UpdatePaginationInfo(int currentPage, int totalPages)
        {
            productMasterPagination.SetPageInfo(currentPage, totalPages);
        }

        private void productMasterPagination_FirstPageClicked(object sender, EventArgs e)
        {
            // Handle first page clicked event
            GetAllProductMasterAsync(1);
        }

        private void productMasterPagination_PreviousPageClicked(object sender, EventArgs e)
        {
            // Handle previous page clicked event
            if (productMasterPagination.CurrentPage > 1)
            {
                --productMasterPagination.CurrentPage;
            }
            int currentIndex = productMasterPagination.CurrentPage;
            GetAllProductMasterAsync(currentIndex);
        }

        private void productMasterPagination_NextPageClicked(object sender, EventArgs e)
        {
            // Handle next page clicked event
            if (productMasterPagination.CurrentPage != productMasterPagination.TotalPages)
            {
                var currentIndex = ++productMasterPagination.CurrentPage;
                GetAllProductMasterAsync(currentIndex);
            }
        }

        private void productMasterPagination_LastPageClicked(object sender, EventArgs e)
        {
            // Handle last page clicked event
            int totalPages = productMasterPagination.TotalPages;
            GetAllProductMasterAsync(totalPages);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = productMasterPagination.CurrentPage = 1;
            int.TryParse(cmbControlType.SelectedValue?.ToString(), out int controlTypeId);
            var controlValue = txtControlValue.Text.Trim().ToTitleCase();
            var isActive = chkIsActive.IsChecked;

            GetAllProductMasterAsync(currentIndex, controlTypeId, controlValue, (bool)isActive);
        }
        private void txtProductSearch_KeyUp(object sender, KeyEventArgs e)
        {
            int currentIndex = productMasterPagination.CurrentPage = 1;
            var controlName = txtProductSearch.Text.Trim().ToTitleCase();

            GetAllProductMasterAsync(currentIndex, 0, controlName, true);
        }

        private void txtProductSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                int currentIndex = productMasterPagination.CurrentPage = 1;
                var controlName = txtProductSearch.Text.Trim().ToTitleCase();

                GetAllProductMasterAsync(currentIndex, 0, controlName, true);
            }
        }
    }
}
