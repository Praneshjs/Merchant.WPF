using Merchant.Helper;
using MerchantDAL.Models;
using MerchantService.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Merchant.Controls
{
    /// <summary>
    /// Interaction logic for SalesControl.xaml
    /// </summary>
    public partial class SalesControl : UserControl
    {
        public SalesControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            GetAllProductAsync(string.Empty, true);
        }

        private void btnProductSearch_Click(object sender, RoutedEventArgs e)
        {
            var controlName = txtProductSearch.Text.Trim().ToLower();
            GetAllProductAsync(controlName, true);
        }

        private void txtProductSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var controlName = txtProductSearch.Text.Trim().ToLower();
            GetAllProductAsync(controlName, true);
        }

        private async void GetAllProductAsync(string allInfo = null, bool? isActive = null)
        {
            ProductService fetchService = new ProductService();
            var allData = await fetchService.GetProductAsync(allInfo, isActive);
            var groupedData = allData
                .GroupBy(p => new { p.FullProductName, p.WeightTypeId, p.SellingPrice })
                .Select(group => new ProductWithCount
                {
                    Product = group.First(),
                    AvailableQuantity = group.Count()
                })
                .OrderBy(t => t.Product.FullProductName)
                .ToList();
            lstProductSearch.ItemsSource = null;
            lstProductSearch.ItemsSource = groupedData;
        }

        private void btnAddProductItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProductWithCount selectedData)
            {
                var container = ListViewHelper.FindAncestor<ListViewItem>(button);
                var textBox = ListViewHelper.FindChild<TextBox>(container, "txtQuantityInGrid");
                decimal.TryParse(textBox.Text, out decimal quantity);
                if (quantity == 0) quantity = 1;
                var purchaseItems = UserSession.Instance.SalesManager.GetAllSales(1);
                bool isItemNotFound = true;
                foreach(var item in purchaseItems)
                {
                    if(item.ProductId == selectedData.Product.Id
                        && item.BrandId == selectedData.Product.BrandId
                        && item.ProductTypeId == selectedData.Product.ProductTypeId
                        && item.Weight == selectedData.Product.ItemWeight
                        && item.SellingPrice == selectedData.Product.SellingPrice)
                    {
                        var availableQuantity = selectedData.AvailableQuantity;
                        var requiredQuantity = item.Quantity + quantity;
                        if (requiredQuantity <= availableQuantity)
                        {
                            item.Quantity = requiredQuantity;
                            isItemNotFound = false;
                        }
                        else
                        {
                            validationMsgCtrl.ShowValidationBox($"{selectedData.Product.FullProductName} available Quantity {availableQuantity}");
                            return;
                        }
                    }
                }

                if (isItemNotFound)
                {
                    SalesItemModel sale = new SalesItemModel
                    {
                        ProductId = selectedData.Product.Id,
                        BrandId = selectedData.Product.BrandId,
                        ProductTypeId = selectedData.Product.ProductTypeId,
                        FullProductName = selectedData.Product.FullProductName,
                        SellingPrice = selectedData.Product.SellingPrice,
                        Weight = selectedData.Product.ItemWeight,
                        ProductWeight = selectedData.Product.ProductWeight,
                        Quantity = quantity
                    };

                    UserSession.Instance.SalesManager.AddSale(1, sale);
                    purchaseItems = UserSession.Instance.SalesManager.GetAllSales(1);
                }
                AssignPurchaseListItemsSource(purchaseItems);
            }
        }

        private void btnRemovePurchaseItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is SalesItemModel selectedData)
            {
                var purchaseItems = UserSession.Instance.SalesManager.GetAllSales(1);
                var itemRemove = purchaseItems.FirstOrDefault(item => item.ProductId == selectedData.ProductId
                       && item.BrandId == selectedData.BrandId
                       && item.ProductTypeId == selectedData.ProductTypeId
                       && item.Weight == selectedData.Weight
                       && item.SellingPrice == selectedData.SellingPrice);
                UserSession.Instance.SalesManager.RemoveSale(1, itemRemove);
                purchaseItems = UserSession.Instance.SalesManager.GetAllSales(1);
                AssignPurchaseListItemsSource(purchaseItems);
            }
        }

        private void AssignPurchaseListItemsSource(List<SalesItemModel> purchaseItems)
        {
            SerialNumberConverter serialNumberConverter = (SerialNumberConverter)this.FindResource("SerialNumberConverter");
            serialNumberConverter.ResetCounter();
            lstPurchaseList.ItemsSource = null;
            lstPurchaseList.ItemsSource = purchaseItems;
        }
    }
}
