using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using WpfApp6.DB;

namespace WpfApp6.Windows
{
    public partial class AddWindow : Window
    {
        public DB.Product newProduct { get; set; } = new DB.Product();
        public AddWindow()
        {
            InitializeComponent();
            cmbCategory.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Windows.AdminWindow.categories
            });
            cmbManufacturer.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Windows.AdminWindow.connection.Manufacturer.ToList()
            });
            cmbSupplier.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Windows.AdminWindow.suppliers
            });
            cmbUnit.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Windows.AdminWindow.units
            });

            DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal price = 0;
                int quantity = 0;
                int discount = 0;
                int maxDiscount = 0;
                if (decimal.TryParse(txtPrice.Text, out price)
                    && decimal.Parse(txtPrice.Text) >= 0
                    && int.TryParse(txtQuantity.Text, out quantity)
                    && int.Parse(txtQuantity.Text) >= 0
                    && int.TryParse(txtDiscount.Text, out discount)
                    && int.Parse(txtDiscount.Text) >= 0
                    && int.TryParse(txtmaxDiscount.Text, out maxDiscount)
                    && int.Parse(txtmaxDiscount.Text) >= 0)
                {
                    var result = MessageBox.Show("Вы точно хотите добавить продукт?", "Уведомелние", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {

                        Windows.AdminWindow.connection.Product.Add(newProduct);
                        int res = Windows.AdminWindow.connection.SaveChanges();
                        if (res != 0)
                        {
                            Windows.AdminWindow.products.Add(newProduct);
                            stProduct.GetBindingExpression(DataContextProperty).UpdateTarget();
                            MessageBox.Show("Продукт добавлен");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Поле цена, скидка, максимальная скидка и количество должны быть целого типа");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Исправьте ошибки");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            if (openFile.ShowDialog().Value == true)
            {
                newProduct.ProductPhoto = openFile.SafeFileName;
            }
        }
    }
}
