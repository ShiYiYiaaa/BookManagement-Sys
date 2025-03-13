using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Windows.Automation;

namespace 图书管理系统
{
    /// <summary>
    /// ManagerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ManagerPage : Window
    {
        public ManagerPage()
        {
            InitializeComponent();
            LoadDataFormFile();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Input.Visibility = Visibility.Visible;
            Output.Visibility = Visibility.Collapsed;
            Back.Visibility = Visibility.Collapsed;

            MessageBox.Show("开始入库");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string id = ID.Text;
            string publisher = Publisher.Text;
            string author = Author.Text;
            string date = Date.Text;
            string amount = Amount.Text;
            string name = Name.Text;

            string bookData = $"{id},{name},{author},{publisher},{amount},{date}";

            try
            {
                File.AppendAllText("BooksData.txt", bookData + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入文件时出错：{ex.Message}");
            }


            if (AreTextBoxesEmptyOrInvalid())
            {
                MessageBox.Show("请输入完整数据。");
            }
            else
            {
                Button_Click(sender, e);
                MessageBox.Show("入库成功。");
            }
        }



        private bool AreTextBoxesEmptyOrInvalid()
        {
            // 检查 ID 是否为空或无效
            if (string.IsNullOrWhiteSpace(ID.Text))
                return true;

            // 检查 Publisher 是否为空或无效
            if (string.IsNullOrWhiteSpace(Publisher.Text))
                return true;

            // 检查 Author 是否为空或无效
            if (string.IsNullOrWhiteSpace(Author.Text))
                return true;

            // 检查 Date 是否为空或无效
            if (string.IsNullOrWhiteSpace(Date.Text))
                return true;

            // 检查 Amount 是否为空或无效
            if (string.IsNullOrWhiteSpace(Amount.Text))
                return true;

            // 检查 Name 是否为空或无效
            if (string.IsNullOrWhiteSpace(Name.Text))
                return true;

            // 如果所有 TextBox 都不为空或无效，则返回 false
            return false;
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("是否要退出入库", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ID.Clear();
                Publisher.Clear();
                Author.Clear();
                Date.Clear();
                Amount.Clear();
                Name.Clear();

                Input.Visibility = Visibility.Collapsed;
            }
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("是否取消出库", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                IDout.Clear();
                Amountout.Clear();
            }
        }



        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string id = IDout.Text;
            string amountStr = Amountout.Text;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(amountStr))
            {
                MessageBox.Show("请输入有效的书籍ID和出库数量！");
                return;
            }

            if (!int.TryParse(amountStr, out int amount))
            {
                MessageBox.Show("请输入有效的出库数量！");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines("BooksData.txt");

                string tempFile = System.IO.Path.GetTempFileName();

                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 6 && parts[0] == id)
                        {
                            if (!int.TryParse(parts[4], out int currentAmount))
                            {
                                MessageBox.Show("书籍数量格式错误！");
                                return;
                            }

                            int remaiiningAmount = currentAmount - amount;

                            parts[4] = remaiiningAmount.ToString();

                            writer.WriteLine(string.Join(",", parts));

                            IDoutshow.Text = parts[0];
                            Publisherout.Text = parts[1];
                            Authorout.Text = parts[2];
                            Dateout.Text = parts[3];
                            Amountoutshow.Text = parts[4];
                            Nameout.Text = parts[5];
                        }

                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                File.Delete("BooksData.txt");
                File.Move(tempFile, "BooksData.txt");

                MessageBox.Show("出库成功");
            }

            catch (Exception ex)
            {
                MessageBox.Show($"读取文件时出错：{ex.Message}");
            }
        }


        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            IDoutshow.Clear();
            IDout.Clear();
            Amountout.Clear();
            Nameout.Clear();
            Publisherout.Clear();
            Authorout.Clear();
            Dateout.Clear();
            Amountoutshow.Clear();

            Output.Visibility = Visibility.Collapsed;

        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string searchID = IDshow.Text;

            if (!int.TryParse(searchID, out _))
            {
                MessageBox.Show("请输入有效的书籍ID（数字）！");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines("BooksData.txt");
                foreach (string lien in lines)
                {
                    string[] parts = lien.Split(',');
                    if (parts.Length >= 6 && parts[0] == searchID)
                    {
                        IDshow.Text = parts[0];
                        Publishershow.Text = parts[3];
                        Authorshow.Text = parts[2];
                        Dateshow.Text = parts[5];
                        Amountshow.Text = parts[4];
                        Nameshow.Text = parts[1];

                        MessageBox.Show("查询成功！");
                        return;
                    }
                }

                MessageBox.Show("未找到对应ID");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取文件时出错：{ex.Message}");
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("是否退出查询？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                IDshow.Clear();
                Publishershow.Clear();
                Authorshow.Clear();
                Dateshow.Clear();
                Nameshow.Clear();
                Amountshow.Clear();

                Find.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {


            MessageBox.Show("请输入ID号");

            Show.Visibility = Visibility.Collapsed;
            Find.Visibility = Visibility.Visible;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Change.Visibility = Visibility.Collapsed;

            MessageBox.Show("开始删除入库信息");

            Delete.Visibility = Visibility.Visible;
        }




        private void DeleteBookData(string idToDelete)
        {
            string id = IDdelete.Text.Trim();
            string publisher = Publisherdelete.Text.Trim();
            string author = Authordelete.Text.Trim();
            string date = Datedelete.Text.Trim();
            string name = Namedelete.Text.Trim();

            try
            {
                string[] lines = File.ReadAllLines("BooksData.txt");

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] parts = line.Split(',');

                    if (parts.Length == 6 && parts[0] == id && parts[1] == name && parts[2] == author && parts[3] == publisher && parts[5] == date)
                    {
                        parts[4] = "0";
                        lines[i] = string.Join(",", parts);

                        File.WriteAllLines("BooksData.txt", lines);

                        Amountdelete.Text = "0";

                        MessageBox.Show("书籍信息匹配成功，并已清零数量。");
                        return;
                    }
                }
                MessageBox.Show("未找到匹配的书籍信息。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取书籍数据时出错：" + ex.Message);
            }
        }



        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            string idToDelete = IDToDelete.Content.ToString();  // 修正变量名大小写

            DeleteBookData(idToDelete);
        }





        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("是否退出？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                IDdelete.Clear();
                Publisherdelete.Clear();
                Authordelete.Clear();
                Datedelete.Clear();
                Namedelete.Clear();
                Amountdelete.Clear();

                Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            Output.Visibility = Visibility.Visible;
            Input.Visibility = Visibility.Collapsed;
            Back.Visibility = Visibility.Collapsed;

            MessageBox.Show("开始出库。");
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
            Change.Visibility = Visibility.Visible;

            MessageBox.Show("开始修改。");
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)//修改按钮
        {
            string idToMatch = IDChange.Text;
            string filePath = "BooksData.txt";
            string[] lines;

            if (File.Exists(filePath))
            {
                lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length >= 1 && parts[0] == idToMatch)
                    {
                        if (parts.Length >= 5)
                        {
                            parts[1] = NameChange.Text;
                            parts[2] = AuthorChange.Text;
                            parts[3] = PublisherChange.Text;
                            parts[5] = DateChange.Text;

                            lines[i] = string.Join(",", parts);
                            File.WriteAllLines(filePath, lines);

                            MessageBox.Show("数据更改成功！");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("数据格式错误");
                            return;
                        }
                    }
                }
                MessageBox.Show("未找到对应ID");
            }
            else
            {
                MessageBox.Show("未找到匹配的书籍信息。");
            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("是否退出修改？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);

            IDChange.Clear();
            NameChange.Clear();
            AuthorChange.Clear();
            PublisherChange.Clear();
            DateChange.Clear();

            Change.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            string idToMatch = IDChange.Text;
            string filePath = "BooksData.txt";

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 1 && parts[0] == idToMatch)
                    {
                        if (parts.Length >= 5)
                        {
                            NameChange.Text = parts[1];
                            AuthorChange.Text = parts[2];
                            PublisherChange.Text = parts[3];
                            DateChange.Text = parts[5];
                        }
                        else
                        {
                            MessageBox.Show("请输入正确的数据格式。");
                        }
                        return;
                    }
                }

                MessageBox.Show("未找到对应ID数据");
            }
            else
            {
                MessageBox.Show("未找到对应文件");
            }
        }


        private void LoadDataFormFile()
        {
            string filePath = "BooksData.txt";

            if (File.Exists(filePath))
            {
                List<Book> books = new List<Book>();

                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(",");
                    if (parts.Length >= 5)
                    {
                        Book book = new Book
                        {
                            书籍ID = parts[0],
                            书籍名称 = parts[1],
                            作者 = parts[2],
                            出版社 = parts[3],
                            库存数量 = parts[4],
                            出版日期 = parts[5]
                        };
                        books.Add(book);
                    }
                    else
                    {
                        MessageBox.Show("格式错误！");
                    }
                }
                DataGrid.ItemsSource = books;
                DataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("未找到文件");
            }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("正在读取。。。");

            Find.Visibility = Visibility.Collapsed;
            Show.Visibility = Visibility.Visible;
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            Show.Visibility = Visibility.Collapsed;
        }



        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            Output.Visibility = Visibility.Collapsed;
            Input.Visibility = Visibility.Collapsed;

            MessageBox.Show("输入ID以归还书本");

            Back.Visibility = Visibility.Visible;
            
        }




        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            string id = IDback.Text;
            string amountStr = Amountback.Text;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(amountStr))
            {
                MessageBox.Show("请输入有效的书籍ID和出库数量！");
                return;
            }

            if (!int.TryParse(amountStr, out int amount))
            {
                MessageBox.Show("请输入有效的出库数量！");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines("BooksData.txt");

                string tempFile = System.IO.Path.GetTempFileName();

                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 6 && parts[0] == id)
                        {
                            if (!int.TryParse(parts[4], out int currentAmount))
                            {
                                MessageBox.Show("书籍数量格式错误！");
                                return;
                            }

                            int remaiiningAmount = currentAmount + amount;

                            parts[4] = remaiiningAmount.ToString();

                            writer.WriteLine(string.Join(",", parts));

                            Publisherback.Text = parts[3];
                            Authorback.Text = parts[2];
                            Dateback.Text = parts[5];
                            Amountbackshow.Text = parts[4];
                            Nameback.Text = parts[1];
                        }

                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                File.Delete("BooksData.txt");
                File.Move(tempFile, "BooksData.txt");

                MessageBox.Show("归还成功");
            }

            catch (Exception ex)
            {
                MessageBox.Show($"读取文件时出错：{ex.Message}");
            }
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            IDback.Clear();
            Nameback.Clear();
            Amountback.Clear();
            Amountbackshow.Clear();
            Dateback.Clear();
            Publisherback.Clear();
            Authorback.Clear();

            MessageBox.Show("退出归还");

            Back.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("正在统计");

            LoadData();
        }


        private void LoadData()
        {
            try
            {
                string booksDataPath = "BooksData.txt";
                int totalBooks = 0;
                int totalLinesBooks = 0;

                foreach (string line in File.ReadAllLines(booksDataPath))
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 5 && int.TryParse(parts[4], out int bookCount))
                        {
                            totalBooks += bookCount;
                            totalLinesBooks++;
                        }
                    }
                }

                Amounttotal.Text = totalBooks.ToString();

                IDtotal.Text = totalLinesBooks.ToString();

                string usersDataPath = "UsersAccount.txt";
                int totalUsers = File.ReadAllLines(usersDataPath).Length;

                Usertotal.Text = totalUsers.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误；" + ex.Message);
            }
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            Amounttotal.Clear();
            IDtotal.Clear();
            Usertotal.Clear();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 获取选定的项
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;

            // 根据选择的项执行相应的操作
            switch (selectedItem.Content.ToString())
            {
                case "帮助":
                    OpenTxtFile("Help.txt");
                    break;
                case "制作人员":
                    OpenTxtFile("Help.txt");
                    break;
                case "隐私声明":
                    MessageBox.Show("本软件源代码由ChatGPT提供帮助。若有侵权，请点击“帮助”联系制作人。");
                    break;
                case "留下反馈":
                    MessageBox.Show("请E-Mail至：1572244807@qq.com 或 ShiYiYiaaa@outlook.com");
                    break;
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;

            switch (selectedItem.Content.ToString())
            {
                case "退出":
                    Close();
                    MainWindow mainWindow1 = new MainWindow();
                    mainWindow1.Close();
                    break;
                case "返回登录":
                    Close();  // 关闭当前窗口
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();  // 显示主窗口
                    break;
                case "刷新页面":
                    ClearAllTextBoxes();
                    break;
            }
        }


        private void ClearAllTextBoxes()
        {
            foreach (var control in GetAllTextBoxes(this))
            {
                ((TextBox)control).Clear();
            }
        }

        private IEnumerable<Control>GetAllTextBoxes(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox)
                {
                    yield return (Control)child;
                }
                else
                {
                    foreach (var descendant in GetAllTextBoxes(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        private void OpenTxtFile(string filename)
        {
            try
            {
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

                System.Diagnostics.Process.Start("notepad.exe",filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开文件: " + ex.Message);
            }
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("正在刷新。");
            LoadDataFormFile();
        }
    }


    public class Book
    {
        public string 书籍ID { get; set; }
        public string 书籍名称 { get; set; }
        public string 作者 { get; set; }
        public string 出版社 { get; set; }
        public string 库存数量 { get; set; }
        public string 出版日期 { get; set; }
    }
}

