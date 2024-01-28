using BobrPraktik.BobrDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
    public partial class UsersWindow : Window
    {
        EmployeeTableAdapter employee = new EmployeeTableAdapter();
        AuthorizationnTableAdapter authorization = new AuthorizationnTableAdapter();
        RoleTableAdapter role = new RoleTableAdapter();
        private int id_authED;
        public UsersWindow()
        {
            InitializeComponent();
            EmployeeDataGrid.ItemsSource = employee.GetData();
            roleCbx.ItemsSource = role.GetData();
            roleCbx.DisplayMemberPath = "Название";
            roleCbx.SelectedValuePath = "ID";

        }
        private void ExitBT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void WarehouseBT_Click(object sender, RoutedEventArgs e)
        {
            WarehouseWindow warehouse = new WarehouseWindow();
            warehouse.Show();
            this.Close();
        }

        private void ProductBT_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow product = new ProductWindow();
            product.Show();
            this.Close();
        }

        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem != null && EmployeeDataGrid.SelectedIndex < EmployeeDataGrid.Items.Count - 1)
            {
                nameTbx.Text = (EmployeeDataGrid.SelectedItem as DataRowView).Row[1].ToString();
                loginTbx.Text = (EmployeeDataGrid.SelectedItem as DataRowView).Row[2].ToString();
                passwordTbx.Text = (EmployeeDataGrid.SelectedItem as DataRowView).Row[3].ToString();
                roleCbx.Text = (EmployeeDataGrid.SelectedItem as DataRowView).Row[4].ToString();
                id_authED = FindId(loginTbx.Text);
            }
        }

        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            if (nameTbx.Text != null && roleCbx.SelectedItem != null && loginTbx.Text != null && passwordTbx.Text != null)
            {
                if (nameTbx.Text.Length <= 35)
                {
                    if ((loginTbx.Text.Length >= 4 && loginTbx.Text.Length <= 30) || (passwordTbx.Text.Length >= 4 && passwordTbx.Text.Length <= 30))
                    {
                        var allEmployeeData = authorization.GetData();

                        int sum = 0;
                        Match a = Regex.Match(loginTbx.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase);
                        if (a.Success) sum++;
                        Match b = Regex.Match(passwordTbx.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase);
                        if (b.Success) sum++;
                        Match c = Regex.Match(nameTbx.Text, @"[а-я]$", RegexOptions.IgnoreCase);
                        if (c.Success) sum++;
                        for (int i = 0; i < allEmployeeData.Count; i++)
                            if (allEmployeeData[i][1].ToString() == loginTbx.Text)
                            {
                                sum = 5;
                                MessageBox.Show("Пользователь с таким логином уже существует!");
                                break;
                            }
                            
                        if (sum == 3)
                        {
                            authorization.InsertQuery(loginTbx.Text, passwordTbx.Text, Convert.ToInt32(roleCbx.SelectedValue));
                            employee.InsertQuery(nameTbx.Text, (int)authorization.ScalarQuery());
                            EmployeeDataGrid.ItemsSource = employee.GetData();
                            Clearing();
                        }
                        else if ( sum != 5) MessageBox.Show("Неверный формат полей");
                    }
                    else MessageBox.Show("Количество символов в полях логина и пароля должны быть не меньше 4 и не больше 30");
                }
                else MessageBox.Show("Превышен лимит символов в поле имени");
            }
            else MessageBox.Show("Поля не должны быть пустыми");
        }

        private void EditBT_Click(object sender, RoutedEventArgs e)
        {
            if (nameTbx.Text != null && roleCbx.SelectedItem != null && loginTbx.Text != null && passwordTbx.Text != null)
            {
                if (nameTbx.Text.Length <= 35)
                {
                    if ((loginTbx.Text.Length >= 4 && loginTbx.Text.Length <= 30) || (passwordTbx.Text.Length >= 4 && passwordTbx.Text.Length <= 30))
                    {
                        var allEmployeeData = authorization.GetData();

                        int sum = 0;
                        Match a = Regex.Match(loginTbx.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase);
                        if (a.Success) sum++;
                        Match b = Regex.Match(passwordTbx.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase);
                        if (b.Success) sum++;
                        Match c = Regex.Match(nameTbx.Text, @"[а-я]$", RegexOptions.IgnoreCase);
                        if (c.Success) sum++;
                        for (int i = 0; i < allEmployeeData.Count; i++)
                            if (allEmployeeData[i][1].ToString() == loginTbx.Text && Convert.ToInt32(allEmployeeData[i][0]) != id_authED)
                            {
                                sum = 5;
                                MessageBox.Show("Пользователь с таким логином уже существует!");
                                break;
                            }
                        if (sum == 3)
                        {
                            object id = (EmployeeDataGrid.SelectedItem as DataRowView).Row[0];
                            authorization.UpdateQuery(loginTbx.Text, passwordTbx.Text, Convert.ToInt32(roleCbx.SelectedValue), id_authED);
                            employee.UpdateQuery(nameTbx.Text, (int)authorization.ScalarQuery(), Convert.ToInt32(id));
                            EmployeeDataGrid.ItemsSource = employee.GetData();
                            Clearing();
                        }
                        else if (sum != 5) MessageBox.Show("Неверный формат полей");
                    }
                    else MessageBox.Show("Количество символов в полях логина и пароля должны быть не меньше 4 и не больше 30");
                }
                else MessageBox.Show("Превышен лимит символов в поле имени");
            }
            else MessageBox.Show("Поля не должны быть пустыми");
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem != null)
            {
                object id = (EmployeeDataGrid.SelectedItem as DataRowView).Row[0];
                employee.DeleteQuery(Convert.ToInt32(id));
                authorization.DeleteQuery(id_authED);
                EmployeeDataGrid.ItemsSource = employee.GetData();
                Clearing();
            }
            else MessageBox.Show("Пользователь не выбран");
        }

        private void Clearing()
        {
            nameTbx.Text = null;
            loginTbx.Text = null;
            passwordTbx.Text = null;
            roleCbx.Text = null;
        }

        private int FindId(string login)
        {
            var allData = authorization.GetData();
            for (int i = 0; i < allData.Count; i++)
            {
                if (login == allData[i][1].ToString())
                {
                    return Convert.ToInt32(allData[i][0].ToString());
                }
            }
            return 0;
        }
    }
}
