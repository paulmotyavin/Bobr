using BobrPraktik.BobrDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BobrPraktik
{
    public partial class WarehouseWindow : Window
    {
        WarehouseTableAdapter warehouse = new WarehouseTableAdapter();
        ProductTableAdapter products = new ProductTableAdapter();
        EmployeeTableAdapter employees = new EmployeeTableAdapter();
        public WarehouseWindow()
        {
            InitializeComponent();
            WarehouseDataGrid.ItemsSource = warehouse.GetData();

            productCbx.ItemsSource = products.GetData();
            productCbx.DisplayMemberPath = "Название товара";
            productCbx.SelectedValuePath = "ID";
            employeeCbx.ItemsSource = employees.GetData();
            employeeCbx.DisplayMemberPath = "Имя пользователя";
            employeeCbx.SelectedValuePath = "ID";
        }
        private void ExitBT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void UsersBT_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow users = new UsersWindow();
            users.Show();
            this.Close();
        }

        private void ProductBT_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow product = new ProductWindow();
            product.Show();
            this.Close();
        }

        private void WarehouseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WarehouseDataGrid.SelectedItem != null && WarehouseDataGrid.SelectedIndex < WarehouseDataGrid.Items.Count - 1)
            {
                productCbx.Text = (WarehouseDataGrid.SelectedItem as DataRowView).Row[2].ToString();
                employeeCbx.Text = (WarehouseDataGrid.SelectedItem as DataRowView).Row[3].ToString();
                addressTbx.Text = (WarehouseDataGrid.SelectedItem as DataRowView).Row[1].ToString();
            }
        }

        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            if (productCbx.SelectedItem != null && employeeCbx.SelectedItem != null && addressTbx.Text != null)
            {
                if (addressTbx.Text.Length <= 250)
                {
                    int sum = 0;
                    string patternQ = @"^[А-Яа-я0-9',.]+(?:\s[А-Яа-я0-9',.]+)+$";
                    Match q = Regex.Match(addressTbx.Text, patternQ);
                    if (q.Success) sum++;
                    if (sum == 1)
                    {
                        warehouse.InsertQuery(addressTbx.Text, Convert.ToInt32(productCbx.SelectedValue), Convert.ToInt32(employeeCbx.SelectedValue));
                        WarehouseDataGrid.ItemsSource = warehouse.GetData();
                        Clearing();
                    }
                    else MessageBox.Show("Неверный формат адреса");
                }
                else MessageBox.Show("Превышен лимит символов в поле");
            }
            else MessageBox.Show("Поля не должны быть пустыми");
        }

        private void EditBT_Click(object sender, RoutedEventArgs e)
        {
            if (productCbx.SelectedItem != null && employeeCbx.SelectedItem != null && addressTbx.Text != null)
            {
                if (addressTbx.Text.Length <= 250)
                {
                    int sum = 0;
                    string patternQ = @"^[А-Яа-я0-9',.]+(?:\s[А-Яа-я0-9',.]+)+$";
                    Match q = Regex.Match(addressTbx.Text, patternQ);
                    if (q.Success) sum++;
                    if (sum == 1)
                    {
                        object id = (WarehouseDataGrid.SelectedItem as DataRowView).Row[0];
                        warehouse.UpdateQuery(addressTbx.Text, Convert.ToInt32(productCbx.SelectedValue), Convert.ToInt32(employeeCbx.SelectedValue), Convert.ToInt32(id));
                        WarehouseDataGrid.ItemsSource = warehouse.GetData();
                        Clearing();
                    }
                    else MessageBox.Show("Неверный формат полей");
                }
                else MessageBox.Show("Превышен лимит символов в поле");
            }
            else MessageBox.Show("Поля не должны быть пустыми");
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            if (WarehouseDataGrid.SelectedItem != null)
            {
                object id = (WarehouseDataGrid.SelectedItem as DataRowView).Row[0];
                warehouse.DeleteQuery(Convert.ToInt32(id));
                WarehouseDataGrid.ItemsSource = warehouse.GetData();
                Clearing();
            }
            else MessageBox.Show("Элемент не выбран");
        }

        private void Clearing()
        {
            productCbx.Text = null;
            employeeCbx.Text = null;
            addressTbx.Text = null;
        }
    }
}
