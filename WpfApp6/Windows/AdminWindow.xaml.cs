using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace WpfApp6.Windows
{
    public partial class AdminWindow : Window
    {
        private Windows.AddWindow addWindow;
        private Windows.EditWindow editWindow;
        public class ItemSort
        {
            public string DisplayName { get; set; }
            public string PropertyName { get; set; }
            public bool isAscending { get; set; }
        }
        public static DB.user9Entities connection = new DB.user9Entities();
        public static ObservableCollection<DB.Product> products { get; set; } = new ObservableCollection<DB.Product>(connection.Product.ToList());
        public static ObservableCollection<DB.Manufacturer> manufacturers { get; set; } = new ObservableCollection<DB.Manufacturer>(connection.Manufacturer.ToList());
        public static ObservableCollection<DB.Category> categories { get; set; } = new ObservableCollection<DB.Category>(connection.Category.ToList());
        public static ObservableCollection<DB.Supplier> suppliers { get; set; } = new ObservableCollection<DB.Supplier>(connection.Supplier.ToList());
        public static ObservableCollection<DB.Unit> units { get; set; } = new ObservableCollection<DB.Unit>(connection.Unit.ToList());
        public static ObservableCollection<DB.OrderProduct> orderProducts { get; set; } = new ObservableCollection<DB.OrderProduct>(connection.OrderProduct.ToList());
        public static ObservableCollection<ItemSort> itemSorts { get; set; } = new ObservableCollection<ItemSort>()
        {
            new ItemSort() {DisplayName = "Сбросить", PropertyName = null, isAscending = true },
            new ItemSort() {DisplayName = "Товары от А-Я", PropertyName = "ProductName", isAscending = true },
            new ItemSort() {DisplayName = "Товары от Я-А", PropertyName = "ProductName", isAscending = false },
            new ItemSort() {DisplayName = "По возрастанию цены", PropertyName = "ProductPrice", isAscending = true },
            new ItemSort() {DisplayName = "По убыванию цены", PropertyName = "ProductPrice", isAscending = false },
            new ItemSort() {DisplayName = "По возрастанию остатка", PropertyName = "ProductQuantity", isAscending = true },
            new ItemSort() {DisplayName = "По убыванию остатка", PropertyName = "ProductQuantity", isAscending = false },
        };
        public AdminWindow()
        {
            InitializeComponent();
            LoadUser();
            manufacturers.Insert(0, new DB.Manufacturer() { ManufacturerName = "Все производители" });
            totalCount.Text = connection.Product.Count().ToString();
            currentCount.Text = connection.Product.Count().ToString();
            cmbManufacturer.ItemsSource = manufacturers;
            listView.ItemsSource = products;
            cmbSort.ItemsSource = itemSorts;
            cmbManufacturer.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;
            DataContext = this;
        }

        private void LoadUser()
        {
            var login = Lib.Connector.Login;
            var user = Lib.Connector.GetModel().User.Where(u => u.UserLogin == login).ToList();
            listUser.ItemsSource = user;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            int count = 0;
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            if (view == null) return;
            view.Filter = new Predicate<object>(obj =>
            {
                DB.Product product = obj as DB.Product;
                bool result = true;
                if (txtSearch.Text.Trim().Length > 0)
                {
                    string searchtext = txtSearch.Text.Trim().ToLower();
                    result = product.ProductName.ToLower().Contains(searchtext)
                    || product.ProductManufacturer.ToLower().Contains(searchtext)
                    || product.ProductArticleNumber.ToLower().Contains(searchtext)
                    || product.ProductSupplier.ToLower().Contains(searchtext)
                    || product.ProductCategory.ToLower().Contains(searchtext)
                    || product.ProductDescription.ToLower().Contains(searchtext);
                }
                if (cmbManufacturer.SelectedIndex > 0)
                {
                    var manufacturer = cmbManufacturer.SelectedItem as DB.Manufacturer;
                    result &= product.ProductManufacturer == manufacturer.ManufacturerName;
                }
                return result;
            });
            view.SortDescriptions.Clear();
            if (cmbSort.SelectedIndex > 0)
            {
                ItemSort itemSort = cmbSort.SelectedItem as ItemSort;
                view.SortDescriptions.Add(new SortDescription(itemSort.PropertyName, itemSort.isAscending ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
            count = ViewCounter(view);
            currentCount.Text = count.ToString();
        }

        private int ViewCounter(ICollectionView view)
        {
            int count = 0;
            foreach (var item in view)
            {
                count++;
            }
            return count;
        }
        private void cmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editWindow != null) return;
            var selectedProduct = listView.SelectedItem as DB.Product;
            editWindow = new EditWindow(selectedProduct);
            editWindow.Closed += EditWindow_Closed;
            editWindow.Show();
            listView.SelectedIndex = -1;

        }

        private void EditWindow_Closed(object sender, EventArgs e)
        {
            editWindow = null;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if(addWindow != null) return;
            addWindow = new AddWindow();
            addWindow.Closed += AddWindow_Closed;
            addWindow.Show();
        }

        private void AddWindow_Closed(object sender, EventArgs e)
        {
            addWindow = null;
        }
    }
}
