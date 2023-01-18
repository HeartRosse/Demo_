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
            new ItemSort() {DisplayName = "По возрастанию", PropertyName = "ProductPrice", isAscending = true },
            new ItemSort() {DisplayName = "По убыванию", PropertyName = "ProductPrice", isAscending = false },
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
            Search(txtSearch.Text.Trim());
        }

        private void Search(string substring)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            if (view == null) return;
            int count = 0;
            view.Filter = new Predicate<object>(obj =>
            {
                bool isView = ((DB.Product)obj).ProductName.ToLower().Contains(substring.ToLower());
                if (isView) count++;
                return isView;
            });
            currentCount.Text = count.ToString();
        }

        private void cmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DB.Manufacturer manufacturer = cmbManufacturer.SelectedItem as DB.Manufacturer;
            ManufacturerFilter(manufacturer.ManufacturerName);
        }

        private void ManufacturerFilter(string manufacturerName)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            if (view == null) return;
            view.Filter = new Predicate<object>(obj =>
            {
                if (manufacturerName == "Все производители") return true;
                return ((DB.Product)obj).ProductManufacturer == manufacturerName;
            });
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemSort itemSort = cmbSort.SelectedItem as ItemSort;
            SortByItem(itemSort.PropertyName, itemSort.isAscending);
        }

        private void SortByItem(string propertyName, bool isAscending)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            if (view == null) return;
            view.SortDescriptions.Clear();
            if (propertyName != null)
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, isAscending ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
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
