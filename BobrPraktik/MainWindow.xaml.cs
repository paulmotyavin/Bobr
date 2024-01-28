using BobrPraktik.BobrDataSetTableAdapters;
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

namespace BobrPraktik
{
    public partial class MainWindow : Window
    {
        AuthorizationnTableAdapter adapter = new AuthorizationnTableAdapter();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AuthorizationBT_Click(object sender, RoutedEventArgs e)
        {
            int log = 0;
            var allLoginData = adapter.GetData();
            for (int i = 0; i < allLoginData.Count; i++)
            {
                if (allLoginData[i][1].ToString() == loginTbx.Text && allLoginData[i][2].ToString() == passwordTbx.Password)
                {
                    log = 1;
                    string roleName = (string)allLoginData[i][3];

                    switch (roleName)
                    {
                        case "Администратор":
                            ProductWindow window = new ProductWindow();
                            window.Show();
                            this.Close();
                            break;
                        case "Пользователь":
                            UserWindow storageManager = new UserWindow();
                            storageManager.Show();
                            this.Close();
                            break;
                    }
                    if (log == 1) break;
                }
            }
            if (log == 0)
            {
                MessageBox.Show("Неверное имя пользователя или пароль");
            }
        }
    }
}
