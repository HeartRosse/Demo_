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

namespace WpfApp6
{
    public partial class MainWindow : Window
    {
        private string code = "";
        public bool isActivate = false;
        public static Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            txtLogin.Text = "dlh4o1tzrbse@yahoo.com";
            txtPass.Password = "2L6KZG";
#endif
            string characters = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890!@#$%&*";
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 4; i++)
            {
                var a = random.Next(0, characters.Length);
                code += characters.Substring(a, 1);
                LetterCreate(code, i);
            }
            LineCreator();
            LineCreator();

        }

        private void LineCreator()
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            line.StrokeThickness = 3.5;
            line.X1 = 5;
            line.Y1 = random.Next(0, 41);
            line.X2 = 195;
            line.Y2 = random.Next(0, 41);
            canvas.Children.Add(line);

        }

        private void LetterCreate(string a, int b)
        {
            Label label = new Label();
            label.Content = a[b];
            label.FontSize = 24;
            label.FontWeight = FontWeights.Black;
            label.RenderTransformOrigin = new Point(0.5, 0.5);
            label.RenderTransform = new RotateTransform(random.Next(-20, 31));
            canvas.Children.Add(label);
            Canvas.SetLeft(label, 50 + (b * 20));
        }

        public void ActivateCaptcha()
        {
            txtCaptcha.Visibility = Visibility.Visible;
            canvas.Visibility = Visibility.Visible;
            isActivate = true;
        }
        public void DeactivateCaptcha()
        {
            txtCaptcha.Visibility = Visibility.Collapsed;
            canvas.Visibility = Visibility.Collapsed;
            isActivate = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (isActivate)
            {
                if (txtCaptcha.Text == code)
                {
                    isActivate = false;
                }
                else
                {
                    MessageBox.Show("Неправильная капча");
                    return;
                }
            }
            if (Lib.Connector.Authorization(txtLogin.Text, txtPass.Password) == true)
            {
                var user = Lib.Connector.GetUser();
                switch (user.UserRole)
                {
                    case "Администратор":
                        MessageBox.Show($"Добро пожаловать, {user.UserName} {user.UserPatronymic}!");
                        Windows.AdminWindow adminWindow = new Windows.AdminWindow();
                        adminWindow.Show();
                        this.Close();
                        break;
                    case "Менеджер":
                        MessageBox.Show($"Добро пожаловать, {user.UserName} {user.UserPatronymic}!");
                        Windows.ManagerWindow managerWindow = new Windows.ManagerWindow();
                        managerWindow.Show();
                        this.Close();
                        break;
                    case "Клиент":
                        MessageBox.Show($"Добро пожаловать, {user.UserName} {user.UserPatronymic}!");
                        Windows.ClientWindow clientWindow = new Windows.ClientWindow();
                        clientWindow.Show();
                        this.Close();
                        break;
                }
                DeactivateCaptcha();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
                ActivateCaptcha();
            }
        }
    }
}
