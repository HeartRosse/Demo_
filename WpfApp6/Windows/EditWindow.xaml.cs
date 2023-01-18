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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfApp6.Windows
{

    public partial class EditWindow : Window
    {
        public DB.Product product { get; set; }

        public EditWindow(DB.Product _product)
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
            product = _product;
            DataContext = this;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
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
                try
                {
                    Windows.AdminWindow.connection.SaveChanges();
                    MessageBox.Show("Данные изменены");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Исправьте ошибки");
            }


        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Windows.AdminWindow.orderProducts.Where(o => o.ProductArticleNumber == product.ProductArticleNumber).ToList().Count() == 0)
            {
                var result = MessageBox.Show("Вы точно хотите удалить продукт?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                try
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        Windows.AdminWindow.connection.Product.Remove(product);
                        Windows.AdminWindow.connection.SaveChanges();
                        MessageBox.Show("Товар удален");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Данный товар нельзя удалить");
            }
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            if (openFile.ShowDialog().Value == true)
            {
                product.ProductPhoto = openFile.SafeFileName;
            }
        }
    }
}
