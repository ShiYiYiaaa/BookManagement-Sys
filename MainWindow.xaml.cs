using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 图书管理系统
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ManagerPage newWindowInstance;
        public MainWindow()
        {       
            InitializeComponent();

            loginButton.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loginButton.Click -= Button_Click;

            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            try
            {
                string filePath = "UsersAccount.txt";

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2 && parts[0] == username && parts[1] == password)
                        {
                            if(newWindowInstance== null)
                            {
                                newWindowInstance= new ManagerPage();
                                newWindowInstance.Closed += ManagerPage_Closed;
                            }

                            newWindowInstance.Show();

                            this.Hide();
                            return;
                        }
                    }
                }
                MessageBox.Show("用户名或密码错误，请重新输入！", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取文件时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void ManagerPage_Closed(object sender, EventArgs e)
        {
            newWindowInstance = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ForgetPassword forgetPassword = new ForgetPassword();
            forgetPassword.Show();
            Hide() ;
        }
    }
}