using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PaginationControl.xaml
    /// </summary>
    public partial class PaginationControl : UserControl
    {
        private int _currentPage = 1; // Initialize current page to 1
        public event PropertyChangedEventHandler PropertyChanged;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }
       
        private int _totalPages = 1; // Initialize total pages to 1
        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                if (_totalPages != value)
                {
                    _totalPages = value;
                    OnPropertyChanged(nameof(TotalPages));
                }
            }
        }

        private string _pageInfo;
        public string PageInfo
        {
            get { return _pageInfo; }
            set
            {
                if (_pageInfo != value)
                {
                    _pageInfo = value;
                    OnPropertyChanged(nameof(PageInfo));
                }
            }
        }

        // Rest of your code for the PaginationControl class...
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler FirstPageClicked;
        public event EventHandler PreviousPageClicked;
        public event EventHandler NextPageClicked;
        public event EventHandler LastPageClicked;

        public static readonly DependencyProperty PageInfoProperty =
            DependencyProperty.Register("PageInfo", typeof(string), typeof(PaginationControl), new PropertyMetadata(""));

        public PaginationControl()
        {
            InitializeComponent();
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            FirstPageClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            PreviousPageClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            NextPageClicked?.Invoke(this, EventArgs.Empty);
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            LastPageClicked?.Invoke(this, EventArgs.Empty);
        }

        public void SetPageInfo(int currentPage, int totalPages)
        {
            this._currentPage = currentPage;
            this._totalPages = totalPages;
            UpdatePageInfo();
            SetCursorStatus();
        }

        public void SetCursorStatus()
        {
            if (CurrentPage == 1)
            {
                btnFirst.Cursor = Cursors.No;
                btnPrevious.Cursor = Cursors.No;
                if (TotalPages > 1)
                {
                    btnLast.Cursor = Cursors.Hand;
                    btnNext.Cursor = Cursors.Hand;
                }
                else
                {
                    btnLast.Cursor = Cursors.No;
                    btnNext.Cursor = Cursors.No;
                }
            }
            else if(CurrentPage == TotalPages)
            {
                btnFirst.Cursor = Cursors.Hand;
                btnPrevious.Cursor = Cursors.Hand;
                btnLast.Cursor = Cursors.No;
                btnNext.Cursor = Cursors.No;
            }
            else
            {
                btnFirst.Cursor = Cursors.Hand;
                btnPrevious.Cursor = Cursors.Hand;
                btnLast.Cursor = Cursors.Hand;
                btnNext.Cursor = Cursors.Hand;
            }
        }

        private void UpdatePageInfo()
        {
            PageInfo = $"{_currentPage}/{_totalPages}";
            txtBlock.Text = PageInfo;
        }
    }
}
