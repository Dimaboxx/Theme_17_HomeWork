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
    public partial class NewClient : Window
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

        public DataTable dt_Clients
        {
            get { return null; }
            set
            {
                dtg_Clients.DataContext = value.DefaultView;
            }
        }





        public event Action<string, string, string, bool> newClientEvent;
        public event Action<string,  bool> newOrganisationEvent;
        public NewClient()
        {

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //SqlCommand c = new SqlCommand();
         if (!String.IsNullOrWhiteSpace(tbx_FirstName.Text) && !String.IsNullOrWhiteSpace(tbx_LastName.Text))
            {
                newClientEvent?.Invoke(
                    tbx_FirstName.Text,
                    tbx_MidleName.Text,
                    tbx_LastName.Text,
                    (bool)(cb_GoodHistory.IsChecked));
                    this.Close();
            }
            else
            {
                MessageBox.Show("FirstName и LastName не могут быть пустым!", "Обнаружено пустое поле", MessageBoxButton.OK, MessageBoxImage.Error);


            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MainWindow.wnd_newClient = null;
            //this.DialogResult = false;
        }



        private void Button_AddOrganisationClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbx_OrganistionName.Text))
            {
                newOrganisationEvent?.Invoke(
                    tbx_OrganistionName.Text,
                    (bool)(cb_GoodHistory.IsChecked));
                    this.Close();
            }
            else
                MessageBox.Show("Имя организации не может быть пустым!", "Обнаружено пустое поле", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


}
