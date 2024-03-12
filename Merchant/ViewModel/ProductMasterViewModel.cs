using MerchantDAL.EntityModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Merchant.ViewModel
{
    public class ProductMasterViewModel : INotifyPropertyChanged
    {
        private string _controlType;
        public string ControlType
        {
            get { return _controlType; }
            set { _controlType = value; OnPropertyChanged(); }
        }

        private string _controlValue;
        public string ControlValue
        {
            get { return _controlValue; }
            set { _controlValue = value; OnPropertyChanged(); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); }
        }

        // Collection to bind to DataGrid
        public ObservableCollection<CommonData> CommonData { get; set; } = new ObservableCollection<CommonData>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
