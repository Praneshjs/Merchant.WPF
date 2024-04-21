using Merchant.Helper;
using MerchantDAL.Models;
using MerchantService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                .GroupBy(p => new { p.FullProductName, p.WeightKgs, p.SellingPrice })
                .Select(group => group.First())
                .OrderBy(t => t.FullProductName)
                .ToList();
            lstProductSearch.ItemsSource = null;
            lstProductSearch.ItemsSource = groupedData;
        }

        private void btnAddProductItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProductModel selectedData)
            {
                var container = ListViewHelper.FindAncestor<ListViewItem>(button);
                var textBox = ListViewHelper.FindChild<TextBox>(container, "txtQuantityInGrid");

                if (textBox != null && decimal.TryParse(textBox.Text, out decimal quantity))
                {
                    SalesItemModel sale = new SalesItemModel
                    {
                        ProductId = selectedData.Id,
                        BrandId = selectedData.BrandId,
                        ProductTypeId = selectedData.ProductTypeId,
                        FullProductName = selectedData.FullProductName,
                        SellingPrice = selectedData.SellingPrice,
                        Weight = selectedData.WeightKgs,
                        Quantity = quantity
                    };

                    UserSession.Instance.SalesManager.AddSale(1, sale);
                    var purchaseItems = UserSession.Instance.SalesManager.GetAllSales(1);
                    lstPurchaseList.ItemsSource = null;
                    lstPurchaseList.ItemsSource = purchaseItems;
                }
                else
                {
                    // Handle invalid quantity input
                }
            }
        }
    }
}
