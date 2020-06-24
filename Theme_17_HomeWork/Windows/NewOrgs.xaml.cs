using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogCenterNameSpace;

namespace Theme_17_HomeWork
{
    /// <summary>
    /// Interaction logic for NewClient.xaml
    /// </summary>
    public partial class NewOrgs : Window
    {
        //       public SqlConnection con { get; set; }
        //public DataTable accTypes
        //{
        //    get { return null; }
        //    set
        //    {
        //        cbx_ClientType.DataContext = value.DefaultView;
        //    }
        //}







        public event Action<string, string, string , bool> newOrganisationEvent;
        public NewOrgs()
        {

            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MainWindow.wnd_newClient = null;
            //this.DialogResult = false;
        }



        private void Button_AddOrganisationClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbx_OrganistionName.Text) && !String.IsNullOrWhiteSpace(tbx_BankDetails.Text) && !String.IsNullOrWhiteSpace(tbx_Adress.Text))
            {
                newOrganisationEvent?.Invoke(
                    tbx_OrganistionName.Text,
                    tbx_BankDetails.Text,
                    tbx_Adress.Text,
                    (bool)(cb_GoodHistory.IsChecked));
                    this.Close();
            }
            else
                MessageBox.Show("Имя организации,Реквизиты и адресс не могут быть пустыми!", "Обнаружено пустое поле", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


}
