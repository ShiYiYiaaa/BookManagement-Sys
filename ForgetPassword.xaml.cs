using System;
using System.Collections.Generic;
using System.IO;
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

namespace 图书管理系统
{
    /// <summary>
    /// ForgetPassword.xaml 的交互逻辑
    /// </summary>
    public partial class ForgetPassword : Window
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string newPassword = NewPasswordBox.Password;

            try
            {
                if(CheckAndUpdatePassword(username, newPassword))
                {
                    MessageBox.Show("密码修改成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("用户名不存在，请重新输入！", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改密码时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckAndUpdatePassword(string username, string newPassword)
        {
            // TODO: 检查用户名是否存在，如果存在则更新密码，然后保存到文件或数据库中
            // 这里你需要实现检查用户名是否存在的逻辑，并更新密码的逻辑
            // 如果用户名存在并成功更新密码，则返回 true，否则返回 false

            string filePath = "UsersAccount.txt";

            List<string> lines = File.ReadAllLines(filePath).ToList();
            for(int i=0;i<lines.Count;i++)
            {
                string[] parts = lines[i].Split(',');
                if(parts.Length == 2 && parts[0]==username) 
                {
                    lines[i] = $"{username},{newPassword}";

                    File.WriteAllLines(filePath, lines);
                    return true;
                }
            }

            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
