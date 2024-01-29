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
    public partial class ProductWindow : Window
    {
        ProductTableAdapter products = new ProductTableAdapter();
        public ProductWindow()
        {
            InitializeComponent();
            ProductDataGrid.ItemsSource = products.GetData();
            Clearing();
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

        private void WareHouseBT_Click(object sender, RoutedEventArgs e)
        {
            WarehouseWindow warehouse = new WarehouseWindow();
            warehouse.Show();
            this.Close();
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem != null && ProductDataGrid.SelectedIndex < ProductDataGrid.Items.Count - 1)
            {
                nameTbx.Text = (ProductDataGrid.SelectedItem as DataRowView).Row[1].ToString();
                amountTbx.Text = (ProductDataGrid.SelectedItem as DataRowView).Row[4].ToString();
                priceTbx.Text = (ProductDataGrid.SelectedItem as DataRowView).Row[2].ToString();
                categoryTbx.Text = (ProductDataGrid.SelectedItem as DataRowView).Row[3].ToString();
            }
        }

        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            if (nameTbx.Text != null && amountTbx.Text != null && priceTbx.Text != null && categoryTbx.Text != null)
            {
                if ((nameTbx.Text.Length <= 55) || (categoryTbx.Text.Length <= 75))
                {
                    int sum = 0;
                    string patternQ = @"[\p{Ll}\p{Lt}]+";
                    Match q = Regex.Match(nameTbx.Text, patternQ);
                    if (q.Success) sum++;
                    string patternW = @"^[0-9]*(?:\,[0-9]*)?$";
                    Match w = Regex.Match(priceTbx.Text, patternW);
                    if (w.Success) sum++;
                    string patternE = @"[^0-9]?$";
                    Match ex = Regex.Match(amountTbx.Text, patternE);
                    if (ex.Success) sum++;
                    string patternR = @"[А-Яа-я]";
                    Match r = Regex.Match(categoryTbx.Text, patternR);
                    if (r.Success) sum++;
                    if (sum == 4)
                    {
                        products.InsertQuery(nameTbx.Text, Convert.ToInt32(amountTbx.Text), (decimal)Convert.ToDouble(priceTbx.Text), categoryTbx.Text);
                        ProductDataGrid.ItemsSource = products.GetData();
                        Clearing();
                    }
                    else MessageBox.Show("Неверный формат полей");
                }
                else MessageBox.Show("Превышен лимит символов в поле");
            }
            else MessageBox.Show("Поля не должны быть пустыми");
        }

        private void EditBT_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem != null)
            {
                if (nameTbx.Text != null && amountTbx.Text != null && priceTbx.Text != null && categoryTbx.Text != null)
                {
                    if ((nameTbx.Text.Length <= 55) || (categoryTbx.Text.Length <= 75))
                    {
                        int sum = 0;
                        object id = (ProductDataGrid.SelectedItem as DataRowView).Row[0];
                        string patternQ = @"[\p{Ll}\p{Lt}]+";
                        Match q = Regex.Match(nameTbx.Text, patternQ, RegexOptions.IgnoreCase);
                        if (q.Success) sum++;
                        string patternW = @"^[0-9]*(?:\,[0-9]*)?$";
                        Match w = Regex.Match(priceTbx.Text, patternW, RegexOptions.IgnoreCase);
                        if (w.Success) sum++;
                        string patternE = @"[^0-9]?$";
                        Match ex = Regex.Match(amountTbx.Text, patternE, RegexOptions.IgnoreCase);
                        if (ex.Success) sum++;
                        string patternR = @"[А-Яа-я]";
                        Match r = Regex.Match(categoryTbx.Text, patternR, RegexOptions.IgnoreCase);
                        if (r.Success) sum++;
                        if (sum == 4)
                        {
                            products.UpdateQuery(nameTbx.Text, Convert.ToInt32(amountTbx.Text), (decimal)Convert.ToDouble(priceTbx.Text), categoryTbx.Text, Convert.ToInt32(id));
                            ProductDataGrid.ItemsSource = products.GetData();
                            Clearing();
                        }
                        else MessageBox.Show("Неверный формат полей");
                    }
                    else MessageBox.Show("Превышен лимит символов в поле");
                }
                else MessageBox.Show("Поля не должны быть пустыми");
            }
            else MessageBox.Show("Продукт не выбран");
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem != null)
            {
                object id = (ProductDataGrid.SelectedItem as DataRowView).Row[0];
                products.DeleteQuery(Convert.ToInt32(id));
                ProductDataGrid.ItemsSource = products.GetData();
                Clearing();
            }
            else MessageBox.Show("Продукт не выбран");
        }

        private void Clearing()
        {
            nameTbx.Text = null;
            amountTbx.Text = null;
            priceTbx.Text = null;
            categoryTbx.Text = null;
            amountTbc.Text = FindAmount();
            priceTbc.Text = FindPrice();
        }

        private string FindAmount()
        {
            int amount = 0;
            var allProductData = products.GetData();
            for (int i = 0; i < allProductData.Count; i++)
                amount += Convert.ToInt32(allProductData[i][4]);
            return "Общее количество: " + amount;
        }

        private string FindPrice()
        {
            decimal price = 0;
            var allProductData = products.GetData();
            for (int i = 0; i < allProductData.Count; i++)
                price += Convert.ToDecimal(allProductData[i][2]);
            return "Общая стоимость: " + price;
        }
    }
}
