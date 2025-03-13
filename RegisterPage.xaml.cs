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
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace 图书管理系统
{
    /// <summary>
    /// RegisterPage.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("请输入用户名和密码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("密码不匹配，请重新输入！", "注册失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string filePath = "UsersAccount.txt";

                using(StreamWriter writer = new StreamWriter(filePath,append:true))
                {
                    writer.WriteLine($"{username},{password}");
                }
                MessageBox.Show("注册成功！", "注册成功", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }

            catch(Exception ex)
            {
                MessageBox.Show($"写入文件时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResgisterButton_Click1(object sender, RoutedEventArgs e)
        {
            this.Close();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
