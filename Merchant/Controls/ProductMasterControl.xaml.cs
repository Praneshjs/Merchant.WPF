using MerchantDAL.EntityModel;
using MerchantService.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Merchant.Controls
{
    /// <summary>
    /// Interaction logic for ProductMasterControl.xaml
    /// </summary>
    public partial class ProductMasterControl : UserControl
    {
        public ProductMasterControl()
        {
            InitializeComponent();
            GetAllProductMasterAsync();
        }
        public ICommand SelectCommand { get; }

        private void ExecuteSelectCommand(CommonData selectedData)
        {
            var data = selectedData;
            // Handle the selection logic here, such as accessing the selected data
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ProductMasterService fetch = new ProductMasterService();
            var controlName = txtControlType.Text.Trim();
            var controlValue = txtControlValue.Text.Trim();
            var isActive = chkIsActive.IsChecked;
            var selectedId = txtSelectedId.Text;
            var allData = await fetch.SubmitProductMasterAsync(selectedId, controlName, controlValue, (bool)isActive);
            //((ProductMasterViewModel)DataContext).CommonData.Clear();
            //((ProductMasterViewModel)DataContext).CommonData = new ObservableCollection<CommonData>(allData);
            //productDataGrid.UpdateLayout();
            productDataGrid.ItemsSource = null;
            productDataGrid.ItemsSource = allData;
            if (!string.IsNullOrEmpty(selectedId))
            {
                ClearButton_Click(null, null);
            }
        }

        private async void GetAllProductMasterAsync()
        {
            ProductMasterService fetch = new ProductMasterService();
            var allData = await fetch.GetProductMasterAsync(null, null, null);
            productDataGrid.ItemsSource = null;
            productDataGrid.ItemsSource = allData;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CommonData selectedData)
            {
                txtControlType.Text = selectedData.ControlType;
                txtControlValue.Text = selectedData.ControlValue;
                chkIsActive.IsChecked = selectedData.IsActive;
                txtSelectedId.Text = selectedData.Id.ToString();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtControlType.Text = string.Empty;
            txtControlValue.Text = string.Empty;
            chkIsActive.IsChecked = true;
            txtSelectedId.Text = string.Empty;
            if (sender != null)
                GetAllProductMasterAsync();
        }
    }
}
