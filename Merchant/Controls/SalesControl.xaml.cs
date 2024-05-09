using Merchant.Helper;
using MerchantDAL.Models;
using MerchantService.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            btnNewOrder_Click(null, null);
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
                var currentBillId = UserSession.Instance.SalesManager.GetCurrentBillId();
                var purchaseItems = UserSession.Instance.SalesManager.GetAllSales(currentBillId);
                bool isItemNotFound = true;
                foreach (var item in purchaseItems)
                {
                    if (item.ProductId == selectedData.Product.Id
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

                    UserSession.Instance.SalesManager.AddSale(currentBillId, sale);
                    purchaseItems = UserSession.Instance.SalesManager.GetAllSales(currentBillId);
                }
                ShowPurchaseNetAmount(purchaseItems);
                AssignPurchaseListItemsSource(purchaseItems);
            }
        }

        private void btnRemovePurchaseItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is SalesItemModel selectedData)
            {
                var currentBillId = UserSession.Instance.SalesManager.GetCurrentBillId();
                var purchaseItems = UserSession.Instance.SalesManager.GetAllSales(currentBillId);
                var itemRemove = purchaseItems.FirstOrDefault(item => item.ProductId == selectedData.ProductId
                       && item.BrandId == selectedData.BrandId
                       && item.ProductTypeId == selectedData.ProductTypeId
                       && item.Weight == selectedData.Weight
                       && item.SellingPrice == selectedData.SellingPrice);
                UserSession.Instance.SalesManager.RemoveSale(1, itemRemove);
                purchaseItems = UserSession.Instance.SalesManager.GetAllSales(currentBillId);
                ShowPurchaseNetAmount(purchaseItems);
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

        private void ShowPurchaseNetAmount(List<SalesItemModel> purchaseItems)
        {
            var grossAmount = purchaseItems.Select(t => t.ItemSetPrice).Sum();
            txtBlGrossAmount.Text = grossAmount.ToString();
            txtProductCount.Text = purchaseItems.Count().ToString();
            txtNetItemCount.Text = purchaseItems.Select(s => s.Quantity).Sum().ToString();
            var gstHalf = (grossAmount * (decimal)(1.5 / 100));
            txtBlCGST.Text = gstHalf.ToString("0.00");
            txtBlSGST.Text = gstHalf.ToString("0.00");
            txtBlNetAmount.Text = (grossAmount + (2 * gstHalf)).ToString("0.00");
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            var currentBillId = UserSession.Instance.SalesManager.GetCurrentBillId();
            var purchaseItems = UserSession.Instance.SalesManager.GetAllSales(currentBillId);

        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var currentBillId = UserSession.Instance.SalesManager.GetCurrentBillId();
            var newBillId = sender == null ? 1 : ++currentBillId;
            if (newBillId > 5)
            {
                validationMsgCtrl.ShowValidationBox("Max 5 bill allowed.");
                return;
            }
            UserSession.Instance.SalesManager.SetCurrentBillId(newBillId);
            Label newBillLabel = new Label();
            newBillLabel.Content = $"Bill {newBillId}";

            foreach (UIElement element in BillStackPanel.Children)
            {
                if (element is Label label)
                {
                    label.Background = (Brush)new BrushConverter().ConvertFrom("#b2bec3");
                    label.Foreground = (Brush)new BrushConverter().ConvertFrom("#636e72");
                }
            }

            newBillLabel.Background = (Brush)new BrushConverter().ConvertFrom("#e17055");
            newBillLabel.Foreground = Brushes.White;
            newBillLabel.Height = 25;
            newBillLabel.FontSize = 12;
            newBillLabel.Margin = new Thickness(5, 5, 5, 5);

            BillStackPanel.Children.Add(newBillLabel);
        }

        private void btnClearOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
