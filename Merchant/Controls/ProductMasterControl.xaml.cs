using Merchant.Helper;
using MerchantDAL.EntityModel;
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
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validationMsgCtrl.CloseValidationBox_Click(null, null);
                ProductMasterService fetch = new ProductMasterService();
                var controlName = txtControlType.Text.Trim().ToTitleCase();
                var controlValue = txtControlValue.Text.Trim().ToTitleCase();
                var isActive = chkIsActive.IsChecked;

                StringBuilder validationMsg = new StringBuilder();
                if (string.IsNullOrEmpty(controlName)) validationMsg.AppendLine("Master control type is empty");
                if (string.IsNullOrEmpty(controlValue)) validationMsg.AppendLine("Master control value is empty");
                var isDataExist = await fetch.IsProductMasterExist(controlName, controlValue);
                if (isDataExist) validationMsg.AppendLine($"Master Data: {controlName} and {controlValue} already exist.");
                if (validationMsg.Length > 0)
                {
                    validationMsgCtrl.ShowValidationBox(validationMsg.ToString());
                    return;
                }

                var selectedId = txtSelectedId.Text;
                var allData = await fetch.SubmitProductMasterAsync(selectedId, controlName, controlValue, (bool)isActive);
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

        private async void GetAllProductMasterAsync(int pageIndex, string controlType = null, string controlValue = null, bool? isActive = null)
        {
            ProductMasterService fetch = new ProductMasterService();
            var allData = await fetch.GetProductMasterAsync(controlType, controlValue, isActive);
            BindProductMasterGridData(allData, pageIndex);
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
                txtControlType.Text = selectedData.ControlType;
                txtControlValue.Text = selectedData.ControlValue;
                chkIsActive.IsChecked = selectedData.IsActive;
                txtSelectedId.Text = selectedData.Id.ToString();
            }
        }
        private void Row_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is CommonData rowData)
            {
                // Handle the event here using the row data
                txtControlType.Text = rowData.ControlType;
                txtControlValue.Text = rowData.ControlValue;
                chkIsActive.IsChecked = rowData.IsActive;
                txtSelectedId.Text = rowData.Id.ToString();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtControlType.Text = string.Empty;
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
            var controlName = txtControlType.Text.Trim().ToTitleCase();
            var controlValue = txtControlValue.Text.Trim().ToTitleCase();
            var isActive = chkIsActive.IsChecked;

            GetAllProductMasterAsync(currentIndex, controlName, controlValue, (bool)isActive);
        }
        private void txtProductSearch_KeyUp(object sender, KeyEventArgs e)
        {
            int currentIndex = productMasterPagination.CurrentPage = 1;
            var controlName = txtProductSearch.Text.Trim().ToTitleCase();

            GetAllProductMasterAsync(currentIndex, controlName, controlName, true);
        }

        private void txtProductSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                int currentIndex = productMasterPagination.CurrentPage = 1;
                var controlName = txtProductSearch.Text.Trim().ToTitleCase();

                GetAllProductMasterAsync(currentIndex, controlName, controlName, true);
            }
        }
    }
}
